function documentScript() {
    var shared = polemoniumExtensionSharedSetup();
    var api = shared.api;

    // debugger;
    console.log('staring processDocument');

    setTimeout(() => shared.markSpamHosts(), 500);
}

documentScript();