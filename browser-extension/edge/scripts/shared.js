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

    function put (url, model) {
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
        hostVotes: (hosts) => get('/host/votes', new URLSearchParams(hosts.map(h => ['hosts', h])).toString()),
        setVote: (host, vote) => put('/host/set-vote', { host: host, vote: vote })
    };
}

function PolemoniumSharedSetup() {
    return {
        Enums: {
            voteType: {
                up: 1,
                down: 2
            }
        },
        Api: prunellaApi()
    };
}