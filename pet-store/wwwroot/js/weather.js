$(function () {
    $('.forecast').ready(async function () {
        const mint = document.querySelector(".mint");
        const maxt = document.querySelector(".maxt");
        const date = document.querySelector(".date");
        const icon = document.querySelector(".day .icon");
        const conditions = document.querySelector(".conditions");
        const baseURL = 'https://www.7timer.info/bin/api.pl?lon=34.8516&lat=31.0461&product=civillight&output=json';
        const response = await fetch(baseURL).then(res => res.json());
        const data = response['dataseries'][0];
        var dtime = new Date(Date.now());
        var hours = dtime.getHours();
        mint.textContent = data['temp2m']['min'];
        maxt.textContent = data['temp2m']['max'];
        date.textContent = dtime.toDateString();
        conditions.textContent = data['weather'];
        if (hours < 18 && hours > 6) {
            icon.classList.add(data['weather'].toLowerCase() + "-day");
        }
        else {
            icon.classList.add(data['weather'].toLowerCase() + "-night");
        }
    });
});