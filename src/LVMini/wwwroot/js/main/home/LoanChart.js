import React, { Component } from "react";
import ReactEcharts from 'echarts-for-react';

export default class LoanChart extends Component {
    state = {
        options: null
    }

    componentWillMount = () => {
        this.getOptions();
    }

    getOptions = () => {
        $.getJSON('/Home/LoanPerformanceDataInquire', data => {
            const array = JSON.parse(data);

            this.setState({
                options: {
                    title: {
                        text: 'Loan amounts for the past three years'
                    },
                    tooltip: {},
                    legend: {
                        data: ['Loan amount']
                    },
                    xAxis: {
                        type: 'log'
                    },
                    yAxis: {
                        data: Object.keys(array)
                    },
                    series: {
                        name: 'Loan amount',
                        type: 'bar',
                        data: Object.values(array)
                    }
                }
            });
        });
    }

    render() {
        return this.state.options ? (
            <div className="row well">
                <ReactEcharts option={this.state.options} className="col-lg-12" />
            </div >
        ) : (
            null);
    }
}