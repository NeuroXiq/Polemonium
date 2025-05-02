function documentScript() {
    function prunellaApi () {
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
    
        function put () {
    
        }
    
        return {
            hostVotes: (hosts) => get('/host/votes', new URLSearchParams(hosts.map(h => ['hosts', h])).toString()),
            setVote: (host, vote) => put('/host/set-vote', { host: host, vote: vote })
        };
    }

    var api = prunellaApi();

    function sGetHostsVotesCache(urls) {
        return Promise.resolve([]);
    }

    function sSetHostsVotesCache(hostsVotes) {
        return Promise.resolve();
    }

    function sGetHostsVotesFetch(hosts) {
        if (!hosts || hosts.length === 0) {
            return Promise.resolve([]);
        }

        return api.hostVotes(hosts).then(r => {
            return r;
        });
    }

    function sGetHostsVotes(hosts) {
        if (!hosts || hosts.length === 0) {
            return;
        }

        return sGetHostsVotesCache(hosts)
            .then(hostsFromCache => {
                let notInCacheHosts = hosts.filter(host => {
                    return hostsFromCache.every(d => d.host !== host);
                });

                if (notInCacheHosts.length > 0) {
                    return sGetHostsVotesFetch(notInCacheHosts)
                        .then(hostsFromServer => {
                            let result = [...hostsFromCache, ...hostsFromServer];
                            return sSetHostsVotesCache(hostsFromServer).then(() => result);
                        });
                } else {
                    return cachedData;
                }
            });
    }

    function sGetHostsToMarkSpam(hosts) {
        var result = [];
        // indexeddb locally set as spam

        // data from server
        return sGetHostsVotes(hosts).then(hostsVotes => {
            for (let i = 0; i < hostsVotes.length; i++) {
                let vote = hostsVotes[i];

                if (vote.voteDownCount === 0) {
                    continue;
                }

                result.push(vote.host);
            }

            return result;
        });
    }

    // debugger;
    console.log('staring processDocument');
    let pageLinks = [...document.getElementsByTagName('a')]
        .filter(aEl => {
            // check which links are valid e.g. href has valid url, href != null etc.
            try { let _ = new URL(aEl.href).host; return true; }
            catch { return false; }
        }).map(aEl => {
            return {
                aEl: aEl,
                host: new URL(aEl.href).host
            }
        });

    //get only unique hosts
    let linksHosts = [...new Set(pageLinks.map(pl => pl.host))];

    sGetHostsToMarkSpam(linksHosts).then(hosts => {
        hosts.forEach(host => {
            pageLinks
                .filter(pageLink => pageLink.host === host)
                .forEach(link => { link.aEl.style = 'outline: solid red;' });
        });
    });
}

documentScript();