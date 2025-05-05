console.log('shared');

function polemoniumExtensionSharedSetup() {
    // setup

    var style = document.createElement('style');
    // style.type = 'text/css';
    style.innerHTML = '.polemonium-spam { background-color: rgba(255,0,0,0.5); }';
    document.getElementsByTagName('head')[0].appendChild(style);

    function apiSetup() {
        var apiUrl = 'https://localhost:7292/api'

        function get(url, searchParams) {
            let fetchUrl = apiUrl + url;

            if (searchParams) {
                fetchUrl = fetchUrl + '?' + searchParams;
            }

            return fetch(fetchUrl, {
                method: 'GET'
            }).then(r => r.json());
        }

        function put(url, model) {
            let fetchUrl = apiUrl + url;
            fetch(fetchUrl, {
                method: 'PUT',
                body: JSON.stringify(model),
                headers: {
                    'content-type': 'application/json'
                }
            }).then(r => r.json());
        }

        return {
            hostVotes: (dnsNames) => get('/host/votes', new URLSearchParams(dnsNames.map(h => ['dnsName', h])).toString()),
            setVote: (dnsName, vote) => put('/host/set-vote', { dnsName: dnsName, vote: vote })
        };
    }

    var api = apiSetup();

    function setHostsVotesCache(votes) {
        return Promise.resolve();
    }

    function getHostsVotesCache(urls) {
        return Promise.resolve([]);
    }

    function getHostsVotesCache(hostsVotes) {
        return Promise.resolve([]);
    }

    function getHostsVotesFetch(hosts) {
        if (!hosts || hosts.length === 0) {
            return Promise.resolve([]);
        }

        return api.hostVotes(hosts).then(r => {
            return r;
        });
    }

    function getHostsToMarkAsSpam(hosts) {
        var result = [];

        // indexedb locally set as spam
        // ... code

        return getHostsVotesCache(hosts)
            .then(hostsFromCache => {
                // get from cache or if not in cache get from server

                var notInCacheHosts = hosts.filter(host => {
                    return hostsFromCache.every(d => d.host !== host);
                });

                var promiseFetchData = notInCacheHosts.length > 0 ? getHostsVotesFetch(notInCacheHosts) : Promise.resolve([]);

                let resultPromise = promiseFetchData
                    .then(hostsFromServer => {
                        let result = [...hostsFromCache, ...hostsFromServer];
                        return setHostsVotesCache(hostsFromServer).then(() => result);
                    });

                return resultPromise;
            }).then(hostsVotes => {
                for (let i = 0; i < hostsVotes.length; i++) {
                    let vote = hostsVotes[i];

                    if (vote.voteDownCount === 0) {
                        continue;
                    }

                    result.push(vote.dnsName);
                }

                return result;
            });
    }

    function getSpamHostsFromAnchors(anchorsElArray) {
        var linksInfo = anchorsElArray.filter(aEl => {
            // check which links are valid e.g. href has valid url, href != null etc.
            try { let _ = new URL(aEl.href).host; return true; }
            catch { return false; }
        }).map(aEl => {
            return {
                aEl: aEl,
                dnsName: new URL(aEl.href).host
            }
        });

        let hostsToQuery = [...new Set(linksInfo.map(pl => pl.dnsName))];

        return getHostsToMarkAsSpam(hostsToQuery)
            .then(spamHosts => {
                for (let i = 0; i < linksInfo.length; i++) {
                    linksInfo[i].isSpam = spamHosts.some(h => h === linksInfo[i].dnsName)
                }

                return linksInfo;
            });
    }

    function markGoogleSpamHosts() {
        let linksInfo = [...document.querySelectorAll('span > a')];
        //get only unique hosts

        getSpamHostsFromAnchors(linksInfo)
            .then(infos => {
                infos.forEach(info => {
                    if (info.isSpam && info.aEl.parentElement) {
                        info.aEl.parentElement.classList.add('polemonium-spam');
                    }
                });
            });
    }

    function markBingSpamHosts() {
        let anchorEls = [...document.querySelectorAll('li h2 a')]

        getSpamHostsFromAnchors(anchorEls)
            .then(infos => {
                infos.forEach(info => {
                    let liToAddCss = info.aEl.parentElement;
                    if (info.isSpam && liToAddCss) {
                        liToAddCss.classList.add('polemonium-spam');
                    }
                });
            });
    }

    function markDuckDuckGoSpamHosts() {
        getSpamHostsFromAnchors([...document.querySelectorAll('article a')]).then(infos => {
            infos.forEach(info => {
                let toAddCss = info.aEl.parentElement?.parentElement?.parentElement?.parentElement;
                if (info.isSpam && toAddCss) {
                    toAddCss.classList.add('polemonium-spam');
                };
            });
        });
    }

    function markSpamHosts() {
        setTimeout(markSpamHosts, 2000);
        let host = window.location.host;
        if (host.startsWith('www.google.') || host.startsWith('google.')) {
            markGoogleSpamHosts();
        } else if (host.startsWith('www.bing.') || host.startsWith('bing.')) {
            markBingSpamHosts();
        } else if (host.startsWith('www.duckduckgo.') || host.startsWith('duckduckgo.')) {
            markDuckDuckGoSpamHosts();
        }
    }

    return {
        enums: {
            voteType: {
                up: 1,
                down: 2
            }
        },
        api: api,
        markSpamHosts: markSpamHosts
    };
}