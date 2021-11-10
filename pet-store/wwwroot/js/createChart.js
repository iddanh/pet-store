function createD3BarChart(data, id) {
    const ele_id = "#" + id;
    $(ele_id).empty();

    const dataKeys = Object.keys(data);

    const dataToDisplay = dataKeys.map(key => ({ genre: key, number: data[key] }))

    var margin = { top: 20, right: 20, bottom: 100, left: 60 },
        width = 300 - margin.left - margin.right,
        height = 280;
    var x = d3.scale.ordinal().rangeRoundBands([0, width], 0.5);
    var y = d3.scale.linear().range([height, 0]);

    //draw axis
    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom");

    var yAxis = d3.svg.axis().tickFormat(d3.format("d"))
        .scale(y)
        .orient("left")
        .ticks(dataToDisplay.length)
        .innerTickSize(-width)
        .outerTickSize(0)
        .tickPadding(10);

    var svg = d3.select(ele_id)
        .append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", "translate(60,20)");

    x.domain(dataToDisplay.map(data => data.genre));

    y.domain([0, d3.max(dataToDisplay, data => data.number)]);

    svg.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0,280)")
        .call(xAxis)
        .selectAll("text")
        .style("text-anchor", "middle")
        .attr("dx", "-2.5em")
        .attr("dy", "-.55em")
        .attr("y", 30)
        .attr("transform", "rotate(-40)")
        .style("stroke", "#69a3b2");

    svg.append("g")
        .attr("class", "y axis")
        .call(yAxis)
        .selectAll("text")
        .attr("transform", "rotate(-90)")
        .attr("y", 5)
        .attr("dy", "0.8em")
        .attr("text-anchor", "end")
        .style("stroke", "#69a3b2");

    svg.selectAll("bar")
        .data(dataToDisplay)
        .enter()
        .append("rect")
        .style({ "fill": " #007bff", "shape-rendering": "crispEdges" })
        .attr("x", data => x(data.genre))
        .attr("width", x.rangeBand())
        .attr("y", data => y(data.number))
        .attr("height", data => (height - y(data.number)));

}