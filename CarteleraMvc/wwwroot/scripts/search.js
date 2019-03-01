function keyWordsearch(elem) {

    //place the API KEY inside the brackets AIzaSyBwkd0wKE3VbP66skDCpAuQvd71QI7Vj2o
    gapi.client.setApiKey('AIzaSyBwkd0wKE3VbP66skDCpAuQvd71QI7Vj2o');
    gapi.client.load('youtube', 'v3', function () {
        makeRequest(elem);
    });
}

//I use the youtube API to search a list of videos, in this case I just want the first one to show, 
function makeRequest(elem) {
    var title = $(elem).data('title');
    var year = $(elem).data('year');
    var q = title + " (" + year + ") Trailer";
    var request = gapi.client.youtube.search.list({
        q: q,
        type: "video",
        part: 'snippet',
        maxResults: 1
    });

   
    request.execute(function (response) {
        if (response.error && response.error.code === 403) {
            window.location.href = '/Home/ErrorApi';
        }
        else {
            $('#results').empty()
            var srchItems = response.result.items;
            $.each(srchItems, function (index, item) {
                vidTitle = item.id.videoId;
                var url = 'https://www.youtube.com/embed/' + vidTitle;

                //replace the value of the src to the video url
                $("#ytplayer").attr('src', url);

                //show title on the header of the modal
                $('#testTitle').html(title);

            })
        }
    })
}

