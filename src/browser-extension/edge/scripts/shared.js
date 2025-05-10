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
            hostVotes: (dnsNames) => get('/dnsname/votes', new URLSearchParams(dnsNames.map(h => ['dnsName', h])).toString()),
            setVote: (dnsName, vote) => put('/dnsname/set-vote', { dnsName: dnsName, vote: vote })
        };
    }

    var api = apiSetup();

    async function addHostVotesToCache(votes) {
        let lsName = 'polemonium_dns_votes_cache_v1';
        let cacheString = localStorage.getItem(lsName);
        let cacheObj = { };

        if (cacheString) {
            cacheObj = JSON.parse(cacheString);
        }

        if (Object.keys(cacheObj) > 10000) {
            cacheObj = { };
        }

        votes.forEach(v => { cacheObj[v.dnsName] = v; });

        localStorage.setItem(lsName, JSON.stringify(cacheObj));

        return Promise.resolve();
    }

    async function getHostsVotesFromCache(dnsNames) {
        let lsName = 'polemonium_dns_votes_cache_v1';
        let cacheString = localStorage.getItem(lsName);
        let cacheObj = { };

        if (cacheString) {
            cacheObj = JSON.parse(cacheString);
        }

        var result = [];

        dnsNames.forEach(c => { cacheObj[c] && result.push(cacheObj[c]); });

        return Promise.resolve(result);
    }

    function getHostsVotesFromServer(hosts) {
        if (!hosts || hosts.length === 0) {
            return Promise.resolve([]);
        }

        return api.hostVotes(hosts).then(r => {
            return r;
        });
    }

    async function getHostsToMarkAsSpam(hosts) {
        var result = [];

        // indexedb locally set as spam
        // ... code

        return getHostsVotesFromCache(hosts)
            .then(hostsFromCache => {
                // get from cache or if not in cache get from server

                var notInCacheHosts = hosts.filter(host => {
                    return hostsFromCache.every(d => d.host !== host);
                });

                var promiseFetchData = notInCacheHosts.length > 0 ? getHostsVotesFromServer(notInCacheHosts) : Promise.resolve([]);

                let resultPromise = promiseFetchData
                    .then(hostsFromServer => {
                        let result = [...hostsFromCache, ...hostsFromServer];
                        return addHostVotesToCache(hostsFromServer).then(() => result);
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
        let host = window.location.host;
        if (host.startsWith('www.google.') || host.startsWith('google.')) {
            markGoogleSpamHosts();
        } else if (host.startsWith('www.bing.') || host.startsWith('bing.')) {
            markBingSpamHosts();
        } else if (host.startsWith('www.duckduckgo.') || host.startsWith('duckduckgo.')) {
            // need to refresh because looks like SPA/dynamically reloads html
            setTimeout(markSpamHosts, 2000);
            markDuckDuckGoSpamHosts();
        }
    }

    async function setVote(dnsName, vote) {
        api.setVote(dnsName, vote);

        let lsLocalVotesName = 'polemonium_local_usr_votes_v1';
        let obj = JSON.parse(localStorage.getItem(lsLocalVotesName) || "{ }");

        obj[dnsName] = { dnsName: dnsName, vote: vote };

        localStorage.setItem(lsLocalVotesName, JSON.stringify(obj));
    }

    return {
        enums: {
            voteType: {
                up: 1,
                down: 2
            }
        },
        // api: api,
        markSpamHosts: markSpamHosts,
        setVote: setVote
    };
}