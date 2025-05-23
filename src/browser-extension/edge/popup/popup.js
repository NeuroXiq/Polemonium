window.Polemonium = window.Polemonium || {};
window.Polemonium.state = window.Polemonium.state || {};
window.Polemonium.Shared = polemoniumExtensionSharedSetup();

var getState = () => window.Polemonium.state;
var api = window.Polemonium.Shared.api;
var enums = window.Polemonium.Shared.enums;
var shared = window.Polemonium.Shared;

function onOkClick() {
    let state = getState();
    shared.setVote(state.host, enums.voteType.up);
}

function onSpamClick() {
    let state = getState();
    shared.setVote(state.host, enums.voteType.down);
}

function setup(tab) {
    if (!tab) {
        return;
    }

    let host = null;

    try { host = new URL(tab.url).host }
    catch { host = null; }

    if (!host) {
        return;
    }

    document.querySelector('.current-host').innerHTML = host;

    window.Polemonium.state = {
        host: host
    }
}

function onTabUpdated(e) {
    chrome.tabs.query(
        { active: true, currentWindow: true },
        (tabsArray) => setup(tabsArray?.length > 0 ? tabsArray[0] : null));
}

document.querySelector('.btn-spam').addEventListener('click', onSpamClick);
document.querySelector('.btn-ok').addEventListener('click', onOkClick);

chrome.tabs.onUpdated.addListener(onTabUpdated);
chrome.tabs.onActivated.addListener(onTabUpdated);
onTabUpdated(null);