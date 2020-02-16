window.SongStatus = {
    updateCurrentSong: function (id, title, artist) {
        var currentSong = document.getElementById(id);
        if (title === "None") {
            currentSong.innerText = "Current Song: None";
        } else {
            currentSong.innerText = "Current Song: " + title + " BY " + artist;
        }       
    },
    updateStatus: function (id, status) {
        var currentStatus = document.getElementById(id);
        console.log(status);
        currentStatus.innerText = "Status: " + status;
    }
};