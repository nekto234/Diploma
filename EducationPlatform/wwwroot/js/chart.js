const data = {
    labels: [],
    datasets: [{
        label: 'на курсі',
        data: [],
        backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(255, 206, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(153, 102, 255, 0.2)',
            'rgba(255, 159, 64, 0.2)'
        ],
        borderColor: [
            'rgba(255,99,132,1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)',
            'rgba(153, 102, 255, 1)',
            'rgba(255, 159, 64, 1)'
        ],
        borderWidth: 1,
        fill: false
    }]
};


const chart = {
    labels: [],
    datasets: [{
        label: 'на курсі',
        data: [],
        backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(255, 206, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(153, 102, 255, 0.2)',
            'rgba(255, 159, 64, 0.2)'
        ],
        borderColor: [
            'rgba(255,99,132,1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)',
            'rgba(153, 102, 255, 1)',
            'rgba(255, 159, 64, 1)'
        ],
        borderWidth: 1,
        fill: false
    }]
};



const rating = {
    labels: [],
    datasets: [{
        label: 'мінімальна оцінка за лабораторну',
        data: [],
        backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(255, 206, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(153, 102, 255, 0.2)',
            'rgba(255, 159, 64, 0.2)'
        ],
        borderColor: [
            'rgba(255,99,132,1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)',
            'rgba(153, 102, 255, 1)',
            'rgba(255, 159, 64, 1)'
        ],
        borderWidth: 1,
        fill: false
    }, {
        label: 'максимальна оцінка за лабораторну',
        data: [],
        backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(255, 206, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(153, 102, 255, 0.2)',
            'rgba(255, 159, 64, 0.2)'
        ],
        borderColor: [
            'rgba(255,99,132,1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)',
            'rgba(153, 102, 255, 1)',
            'rgba(255, 159, 64, 1)'
        ],
        borderWidth: 1,
        fill: false
        }, {
            label: 'мінімальна оцінка за тест',
            data: [],
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
                'rgba(255, 159, 64, 0.2)'
            ],
            borderColor: [
                'rgba(255,99,132,1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)'
            ],
            borderWidth: 1,
            fill: false
        }, {
            label: 'максимальна оцінка за тест',
            data: [],
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
                'rgba(255, 159, 64, 0.2)'
            ],
            borderColor: [
                'rgba(255,99,132,1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)'
            ],
            borderWidth: 1,
            fill: false
        }]
};


const options = {
    scales: {
        yAxes: [{
            ticks: {
                beginAtZero: true
            }
        }]
    },
    legend: {
        display: false
    },
    elements: {
        point: {
            radius: 0
        }
    }

};

if ($("#barChart").length) {
    const barChartCanvas = $("#barChart").get(0).getContext("2d");
    // This will get the first returned node in the jQuery collection.

    $.getJSON('/Statistics/GetCoursesBarData', (response) => {
        console.log(response)
        const _labels = [];
        const _data = [];

        response.forEach(x => {
            _labels.push(x.name);
            _data.push(x.students);
        });

        console.log([_data]);
        chart.labels = _labels;
        chart.datasets[0].data = _data;

        const barChart = new Chart(barChartCanvas, {
            type: 'bar',
            data: chart,
            options: options
        });
    });
}

if ($("#lineChart").length) {

    $.getJSON('/Statistics/GetCoursesAvgMarksData', (response) => {
        //console.log(response)
        const _labels = [];
        const _data = [];

        response.forEach(x => {
            _labels.push(x.course);
            _data.push(x.avgMark);
        });

        data.labels = _labels;
        data.datasets[0].data = _data;

        const lineChartCanvas = $("#lineChart").get(0).getContext("2d");
        const lineChart = new Chart(lineChartCanvas, {
            type: 'bar',
            data: data,
            options: options
        });
    });
}



if ($('#moduleChart').length) {

    const lineChartCanvas = $("#moduleChart").get(0).getContext("2d");

    $.getJSON('/Statistics/GetModulesAvgMarkData?courseId=' + courseId, (response) => {

        const _labes = [];
        const _data = [];

        response.forEach(item => {
            _data.push(item.avgMark);
            _labes.push(item.module);
        });

        data.labels = _labes;
        data.datasets[0].data = _data;

        const lineChart = new Chart(lineChartCanvas, {
            type: 'bar',
            data: data,
            options: options
        });
    });
}


if ($('#studentRating').length) {
    const lineChartCanvas = $("#studentRating").get(0).getContext("2d");

    $.getJSON('/Statistics/GetCourseMarkForStudent?courseId=' + courseId, (response) => {
        const _labes = [];
        const _data = [];

        response.forEach(item => {
            _data.push(item.procentRatin);
            _labes.push(item.student.firstName + ' ' + item.student.lastName);
        });

        data.labels = _labes;
        data.datasets[0].data = _data;
        data.datasets[0].label = 'оцінка студента';
        const lineChart = new Chart(lineChartCanvas, {
            type: 'bar',
            data: data,
            options: options
        });
    });
}


if ($('#minAndMaxRating').length) {
    const lineChartCanvas = $("#minAndMaxRating").get(0).getContext("2d");
    $.getJSON('/Statistics/MinMaxRating?courseId=' + courseId, (response) => {
        console.log(response);
        const _labes = [];
        const _minLab = [],
            _maxLab = [],
            _minTest = [],
            _maxTest = [];

        response.forEach(item => {
            _minLab.push(item.minLabMark);
            _maxLab.push(item.maxLabMark);
            _minTest.push(item.minTestMark);
            _maxTest.push(item.maxTestMark);
            _labes.push(item.name);
        });

        rating.datasets[0].data = _minLab.map(x => x == null || x == undefined ? 0 : x);
        rating.datasets[1].data = _maxLab.map(x => x == null || x == undefined ? 0 : x);
        rating.datasets[2].data = _minTest.map(x => x == null || x == undefined ? 0 : x);
        rating.datasets[3].data = _maxTest.map(x => x == null || x == undefined ? 0 : x);

        rating.labels = _labes;
        console.log(rating);
        console.log(rating.datasets[0].data, rating.datasets[1].data, rating.datasets[2].data, rating.datasets[3].data)
        const lineChart = new Chart(lineChartCanvas,
            {
                type: 'bar',
                data: rating,
                options: options
            });
    });
}