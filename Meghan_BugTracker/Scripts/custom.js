// TEMPLATE IN-LINE JS

// -------------------------------------------------------------------------
// Initialize DEMO sidebar

$(function () {
    pxDemo.initializeDemoSidebar();

    $('#px-demo-sidebar').pxSidebar();
    pxDemo.initializeDemo();
});

// -------------------------------------------------------------------------
// Initialize DEMO

$(function() {
    var file = String(document.location).split('/').pop();

    // Remove unnecessary file parts
    file = file.replace(/(\.html).*/i, '$1');

    if (!/.html$/i.test(file)) {
        file = 'index.html';
    }

    // Activate current nav item
    $('body > .px-nav')
    .find('.px-nav-item > a[href="' + file + '"]')
    .parent()
    .addClass('active');

    $('body > .px-nav').pxNav();
    $('body > .px-footer').pxFooter();

    $('#navbar-notifications').perfectScrollbar();
    $('#navbar-messages').perfectScrollbar();
});

// -------------------------------------------------------------------------
// Initialize uploads chart

 $(function() {
    var data = [
    {day: '2014-03-10', v: pxDemo.getRandomData(20, 5) },
    {day: '2014-03-11', v: pxDemo.getRandomData(20, 5) },
    {day: '2014-03-12', v: pxDemo.getRandomData(20, 5) },
    {day: '2014-03-13', v: pxDemo.getRandomData(20, 5) },
    {day: '2014-03-14', v: pxDemo.getRandomData(20, 5) },
    {day: '2014-03-15', v: pxDemo.getRandomData(20, 5) },
    {day: '2014-03-16', v: pxDemo.getRandomData(20, 5) }
    ];

    new Morris.Line({
        element: 'hero-graph',
    data: data,
    xkey: 'day',
    ykeys: ['v'],
    labels: ['Value'],
    lineColors: ['#fff'],
    lineWidth: 2,
    pointSize: 4,
    gridLineColor: 'rgba(255,255,255,.5)',
    resize: true,
    gridTextColor: '#fff',
    xLabels: "day",
    xLabelFormat: function(d) {
        return ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov', 'Dec'][d.getMonth()] + ' ' + d.getDate();
    },
    });
});

// -------------------------------------------------------------------------
// Initialize easy pie charts

$(function() {
    var colors = pxDemo.getRandomColors();

    var config = {
        animate: 2000,
    scaleColor: false,
    lineWidth: 4,
    lineCap: 'square',
    size: 90,
    trackColor: 'rgba(0, 0, 0, .09)',
    onStep: function(_from, _to, currentValue) {
        var value = $(this.el).attr('data-max-value') * currentValue / 100;

        $(this.el)
        .find('> span')
        .text(Math.round(value) + $(this.el).attr('data-suffix'));
    },
    }

    var data = [
    pxDemo.getRandomData(1000, 1),
    pxDemo.getRandomData(100, 1),
    pxDemo.getRandomData(64, 1),
    ];

    $('#easy-pie-chart-1')
    .attr('data-percent', (data[0] / 1000) * 100)
    .attr('data-max-value', 1000)
    .easyPieChart($.extend({}, config, {barColor: colors[0] }));

    $('#easy-pie-chart-2')
    .attr('data-percent', (data[1] / 100) * 100)
    .attr('data-max-value', 100)
    .easyPieChart($.extend({}, config, {barColor: colors[1] }));

    $('#easy-pie-chart-3')
    .attr('data-percent', (data[2] / 64) * 100)
    .attr('data-max-value', 64)
    .easyPieChart($.extend({}, config, {barColor: colors[2] }));
});

// -------------------------------------------------------------------------
// Initialize retweets graph

$(function() {
    var data = [
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    ];

    $("#sparkline-1").pxSparkline(data, {
        type: 'line',
    width: '100%',
    height: '42px',
    fillColor: '',
    lineColor: '#fff',
    lineWidth: 2,
    spotColor: '#ffffff',
    minSpotColor: '#ffffff',
    maxSpotColor: '#ffffff',
    highlightSpotColor: '#ffffff',
    highlightLineColor: '#ffffff',
    spotRadius: 4,
    });
});

// -------------------------------------------------------------------------
// Initialize Monthly visitor statistics graph

$(function() {
    var data = [
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    pxDemo.getRandomData(300, 100),
    ];

    $("#sparkline-2").pxSparkline(data, {
        type: 'bar',
    height: '42px',
    width: '100%',
    barSpacing: 2,
    zeroAxis: false,
    barColor: '#ffffff',
    });
});

// -------------------------------------------------------------------------
// Initialize scrollbars

$(function() {
    $('#support-tickets').perfectScrollbar();
    $('#recent-projects').perfectScrollbar();
    $('#index-comments').perfectScrollbar();
    $('#index-histories').perfectScrollbar();
    $('#threads').perfectScrollbar();
});

//--------------------------------------------------------------------------
// Initialize login page components

$(function () {
    $('.nav-tabs').pxTabResize();
    $('#account-bio').pxCharLimit();
});