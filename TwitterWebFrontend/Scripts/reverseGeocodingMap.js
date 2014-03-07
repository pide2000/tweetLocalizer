var geocoderll;
var mapll;
var infowindow = new google.maps.InfoWindow();
var markerll;
function initializell() {
    geocoderll = new google.maps.Geocoder();
    var latlng = new google.maps.LatLng(40.730885, -73.997383);
    var mapOptionsll = {
        zoom: 8,
        center: latlng,
        mapTypeId: 'roadmap'
    }
    mapll = new google.maps.Map(document.getElementById('map-canvas-lat-lng'), mapOptionsll);
   
}

function codeLtLg() {
    var input = document.getElementById('latlng').value;
    var latlngStr = input.split('.', 2);
    var lat = latlngStr[0].replace(',', '.');
    var lng = latlngStr[1].replace(',', '.');
    var latlng = new google.maps.LatLng(lat, lng);
    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                map.setZoom(11);
                marker = new google.maps.Marker({
                    position: latlng,
                    map: map
                });
                infowindow.setContent(results[1].formatted_address);
                infowindow.open(map, marker);
            } else {
                alert('No results found');
            }
        } else {
            alert('Geocoder failed due to: ' + status);
        }
    });
}

function codeLatLng(ll) {
    var input = ll;
    var latlngStr = input.split('.', 2);
    var la = latlngStr[0].replace(',', '.');
    var ln = latlngStr[1].replace(',', '.');
    var lat = parseFloat(la);
    var lng = parseFloat(ln);
    var latlng = new google.maps.LatLng(lat, lng);
    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                mapll.setZoom(4);
                mapll.setCenter(latlng);
                marker = new google.maps.Marker({
                    position: latlng,
                    map: mapll
                });
                infowindow.setContent(results[1].formatted_address);
                infowindow.open(mapll, marker);
            } else {
                alert('No results found');
            }
        } else {
            alert('Geocoder failed due to: ' + status);
        }
    });
}

google.maps.event.addDomListener(window, 'load', initializell);