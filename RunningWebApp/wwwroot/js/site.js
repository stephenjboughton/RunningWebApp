// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



/**
 * Get a template DOM object from the DOM and return a usable DOM object
 * from the main node within it. Assumes that there is only one main parent
 * node in the template.
 * 
 * @param {string} id the id of the template element
 * @returns {Object} a deep clone of the templated element
 */
function getElementFromTemplate(id) {
    let domNode = document.importNode(document.getElementById(id).content, true).firstElementChild;

    return domNode;
}

document.addEventListener('DOMContentLoaded', () => {
    // Code that runs when the DOM is loaded and verifies we have attached event handlers
    console.log('DOM Loaded');
    // Locate the try it button
    const Start = document.querySelector('button#start');
    const CalculatePace = document.querySelector('button#submit-data');
    // Attach an event handler for a click on the button
    Start.addEventListener('click', (event) => {
        //console.log('Timer button added');    
        getLocation(event.currentTarget);
        //save the timestamp when the user presses start
        startTime = Date.now();
        //declare a variable that returns location every 5s and adds it to the route array
        routeActive = setInterval(getLocation, 5000)
        // run updatetimer every 1s as long as the timer isn't already running (if we click multiple times and don't have this conditional, it will start multiple loops that run every second, creating a bizarre, herky-jerky sped up timer)
        if (runningTime === undefined) {
            runningTime = setInterval(updateTimer, 1000);
        }
        //hide start button
        Start.classList.add('hidden');
        //unhide stop button
        Stop.classList.remove('hidden');
    });
    //locate the stop button and add an event listener for a click
    const Stop = document.querySelector('button#stop');
    const Reset = document.querySelector('button#reset');
    Stop.addEventListener('click', (event) => {
        //stop the repeating interval that adds location to array every 10s
        clearInterval(routeActive);
        //stop the repeating interval that updates the timer display every second
        clearInterval(runningTime);
        //save the timestamp from the moment the user presses stop
        stopTime = Date.now();
        //hide the stop button
        Stop.classList.add('hidden');
        mapTimeDistanceDisplayToForm();
        //show the start button
        Reset.classList.remove('hidden');
        //call the function that assigns values from running timer and distance display to the appropriate form fields to submit to calculator action
        CalculatePace.classList.remove('hidden');

        console.log(distance);
        console.log(displayTime(stopTime - startTime));
    });

    Reset.addEventListener('click', (event) => {
        console.log('Reset button added');
        clearTimer();
        Reset.classList.add('hidden');
        Start.classList.remove('hidden');
    });

    //locate the calculate pace button and use it to submit a for to calculate pace based on the run recorded
    CalculatePace.addEventListener('click', (event) => {
        console.log('Calculate Button added');
        
    });
});
//declare variable to hold the interval function that calls getLocation every 5s
let routeActive;
//declare array to hold each Location ping and variables startTime and stopTime for when user hits start and stop, then calculate elapsed time by subtracting start from stop
let route = [];
let startTime = new Date();
let stopTime = new Date();
let elapsed = new Date();
let totalElapsedTime = stopTime - startTime;
let distance;

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        x.innerText = "Geolocation is not supported by this browser.";
    }
}

function showPosition(position) {
    let wayPoint = {};
    let time;
    wayPoint.lat = position.coords.latitude;
    wayPoint.lon = position.coords.longitude;
    wayPoint.time = Date.now();
    route.push(wayPoint);
    console.log(route);

    /*let newPosition = getElementFromTemplate('wP');
     newPosition.querySelector('.demoLat').innerText = wayPoint.lat;
     newPosition.querySelector('.demoLon').innerText = wayPoint.lon;
     newPosition.querySelector('.wPTime'). innerText = displayTime(wayPoint.time);
     document.querySelector('table#routeWP tbody').insertAdjacentElement('afterbegin', newPosition);*/
}

//variable used to access the timer that is running once we click start
let runningTime;
//variable used to access the distance display that lets the user know how far they have gone
const distanceDisplay = document.querySelector('h2#distanceCovered');


// function that locates the running time display, assigns the value of its .time property to the runningTime variable, then updates the display on an interval. this function also updates the distance display and updates it to hold the current distance traveled as caluculated by returnDistance function, which should run every time we add a new waypoint to our route (~ every 5 seconds) (and that value is truncated to two decimal places)
function updateTimer() {
    const runningTimer = document.getElementById('runningTimer');
    let elapsed = Date.now() - startTime;
    runningTimer.dataset.time = elapsed;
    runningTimer.innerText = displayTime(runningTimer.dataset.time);
    distance = returnDistance(route);
    distanceDisplay.dataset.distance = distance;
    distanceDisplay.innerText = distance.toFixed(2) + " mi";
}

//function that will reset the values of the timer
function clearTimer() {
    const runningTimer = document.getElementById('runningTimer');
    runningTimer.dataset.time = 0;
    runningTimer.innerText = "00:00:00";
    distanceDisplay.innerText = "";
    document.querySelector('table#routeWP tbody').innerHTML = '<template id="wP"><tr><td class="demoLat"></td><td class="demoLon"></td><td class="wPTime"></td></tr></template>';
}

//function that assigns the time elements and distance from our run recorder to the appropriate fields of a rundata form which we can submit to our calculator action in order to return a pace
function mapTimeDistanceDisplayToForm() {
    const hoursToSubmit = document.querySelector('input#hours-recorded');
    hoursToSubmit.setAttribute("value", calculateHours(runningTimer.dataset.time));
    const minutesToSubmit = document.querySelector('input#minutes-recorded');
    minutesToSubmit.setAttribute("value", calculateMinutes(runningTimer.dataset.time));
    const secondsToSubmit = document.querySelector('input#seconds-recorded');
    secondsToSubmit.setAttribute("value", calculateSeconds(runningTimer.dataset.time));
    const milesToSubmit = document.querySelector('input#distance-recorded');
    milesToSubmit.setAttribute("value", distanceDisplay.dataset.distance);
}

/* NEXT TO DO - refactor showPosition to use an array of objects, then add a timestamp to each waypoint - may need to order them later on because of asynchronous nature of js execution - somoe of them will return out of order possibly.  also, base total run time off timestamp from time of start  button click and timestamp from time of stop button click, not interval, and probably base display of timer off of timestamps too.*/

//let googleLatLongs;
//function that takes a route array and assigns the lat and lon values of each object in the route array to a google LatLong object, then feeds that array into google maps api compute length function to return the distance run.
function returnDistance(route) {
    let googleLatLongs = [];
    for (let i = 0; i < route.length; i++) {
        googleLatLongs.push(new google.maps.LatLng(route[i].lat, route[i].lon));
    }
    let runMeters = google.maps.geometry.spherical.computeLength(googleLatLongs);
    let runMiles = convertToMiles(runMeters);
    return runMiles;
}

function convertToMiles(meters) {
    let miles;
    miles = meters * 0.00062137;
    return miles;
}

const milisecondsPerSecond = 1000;
const secondsPerMinute = 60000;
const secondsPerHour = 3600000;

// function that takes a total time in seconds and breaks it down into hours, minutes and seconds then assigns them to a colon delimited string
function displayTime(time) {
    let hours = Math.floor((time / secondsPerHour)).pad();
    let minutes = Math.floor(((time % secondsPerHour) / secondsPerMinute)).pad();
    let seconds = Math.floor(((time % secondsPerHour) % secondsPerMinute) / milisecondsPerSecond).pad();
    let timeDisplay = `${hours}:${minutes}:${seconds}`;
    return timeDisplay;
}

//group of three functions that calculates each of the three time components needed to bind to the model we submit after recording a run by taking the dataset from our timer element
function calculateHours(time) {
    return Math.floor((time / secondsPerHour));
}
function calculateMinutes(time) {
    return Math.floor(((time % secondsPerHour) / secondsPerMinute));
}
function calculateSeconds(time) {
    return Math.floor(((time % secondsPerHour) % secondsPerMinute) / milisecondsPerSecond);
}

// function that adds leading zeros to the hours, minutes, seconds in the countDown timer
Number.prototype.pad = function (size) {
    var s = String(this);
    while (s.length < (size || 2)) { s = "0" + s; }
    return s;
}