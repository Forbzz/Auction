$(function () {
    Timezone();
});

function Timezone() {
    var value = new Date().getTimezoneOffset();
    console.log(value);
    document.cookie = 'timezoneoffset' + '=' + encodeURI(value);
}