$(document).ready(function() {
    var now = new Date();
    var weatherUrl = 'https://query.yahooapis.com/v1/public/yql?format=json&rnd=' +
        now.getFullYear() + now.getMonth() + now.getDay() + now.getHours() + '&diagnostics=true&callback=?&q=';

    var location = 'Rishon LeZion, IL';
    var unit = 'c';

    weatherUrl +=
        'select * from weather.forecast where woeid in (select woeid from geo.places(1) where text="' + location + '")' +
        ' and u="' + unit + '"';

    $.getJSON(
        encodeURI(weatherUrl),
        function(data) {
            if(data !== null && data.query !== null && data.query.results !== null && data.query.results.channel.description !== 'Yahoo! Weather Error') {
                var result = data.query.results.channel;

                var html = '<ul><li><h2>' +result.item.condition.temp + '&deg;' +unit +'</h2></li>';
                html += '<li>' + result.item.condition.text + ' - ' + result.location.city +'</li>';

                $("#weather").html(html);
            } else {
                $("#weather").html('<p>' + 'Error receiving the weather forecast' + '</p>');
            }
        }
    );
});