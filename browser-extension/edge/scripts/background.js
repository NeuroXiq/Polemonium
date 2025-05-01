console.log('background start');

function googleCreateSpanOutline(tab) {

}

function googleGetAllUrls(tab) {

}

function documentScript() {
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

        // indexed db get

        let qparam = new URLSearchParams(hosts.map(h => ['hosts', h])).toString();
        return fetch('https://localhost:7292/api/website/hosts-votes?' + qparam, {
            method: 'GET'
        }).then(r => {
            return r.json()
        }).then(r => {
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
            try { let _ = new URL(aEl.href).host; return true; }
            catch { return false; }
        }).map(aEl => {
            console.log(aEl.href);
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

function processTab(tab) {
    console.log('process tab', tab);

    if (!tab || !tab.url || !tab.url?.startsWith('https://') || tab.status !== 'complete') {
        return;
    }

    let host = (new URL(tab.url)).host;

    if (!host.includes('google')) {
        return;
    }

    chrome.scripting.executeScript({
        target: { tabId: tab.id },
        func: documentScript
    }).then(r => { console.log('end execute script', r); });

    //fetch('https://localhost:7292/api/website/websites-votes')

    //fetch('https://localhost:1234/api/website?q=test.com')
    //.then(r=> console.log(r));
}

function onTabUpdated(e) {
    console.log('tab updated', e);
    chrome.tabs.query(
        { /* active: true, currentWindow: true */ },
        (tabsArray) => processTab(tabsArray?.length > 0 ? tabsArray[0] : null));

    // chrome.tabs.query({ active: true, currentWindow: true }, tabs => {
    //     console.log('query tabs');
    // });

}

chrome.tabs.onUpdated.addListener(onTabUpdated);