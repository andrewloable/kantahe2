window.video = {
    playerTheme: function () {
        document.body.classList.add('player-body');
    },
    adminTheme: function () {
        document.body.classList.add('admin-body');
    },
    play: function (id, url) {
        console.log("PLAYING");
        var videoPlayer = document.getElementById(id);
        var source = document.createElement('source');
        source.setAttribute('src', url);
        videoPlayer.innerHTML = '';
        videoPlayer.appendChild(source);
        videoPlayer.load();
        videoPlayer.play();
    },
    stop: function (id) {
        console.log("STOPPED");
        var videoPlayer = document.getElementById(id);
        videoPlayer.innerHTML = '';
        videoPlayer.pause();
    },
    ended: function () {
        window.video.callbackEnded.invokeMethodAsync('VideoEnded');
    },
    setCallback: function (helper) {
        window.video.callbackEnded = helper;
    },
    test: function (id, msg) {
        var elem = document.getElementById(id);
        elem.innerHTML = msg;
    }
}