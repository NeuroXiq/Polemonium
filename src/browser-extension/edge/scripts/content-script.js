function documentScript() {
    var shared = polemoniumExtensionSharedSetup();
    var api = shared.api;

    // debugger;
    console.log('staring processDocument');

    shared.markSpamHosts();
}

documentScript();