window.video = {
    play: function (id, filename) {
        console.log("PLAYING");
        console.log(filename);
        var videoPlayer = document.getElementById(id);
        var source = document.createElement('source');        
        source.setAttribute('src', filename);
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
        alert("Video Ended");
    },
    test: function (id, msg) {
        var elem = document.getElementById(id);
        elem.innerHTML = msg;
    }
}