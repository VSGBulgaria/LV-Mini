import React, { Component } from "react";
import ReactEcharts from 'echarts-for-react';

export default class BudgetChart extends Component {
    state = {
        options: null
    }

    componentWillMount = () => {
        this.getOptions();
    }

    getOptions = () => {
        $.getJSON('/Home/LoanBudgetVersusActualInquire', data => {
            const array = JSON.parse(data);

            const arrayKeys = Object.keys(array);
            const keys = [];

            arrayKeys.forEach(function (element) {
                const upperCase = element.charAt(0).toUpperCase() + element.slice(1);
                const newKey = upperCase.split(" ").join("\n");

                keys.push(newKey);
            });
            
            const values = Object.values(array);

            const yearlyBudget = [];
            values.forEach(function(element) {
                yearlyBudget.push(element.yearlyBudget);
            });

            const actualBudget = [];
            values.forEach(function (element) {
                actualBudget.push(element.actualBudget);
            });
            debugger;
            this.setState({
                options: {
                    title: {
                        text: 'Loan budget versus actual'
                    },
                    tooltip: {},
                    legend: {
                        data: ['Budget', 'Actual']
                    },
                    xAxis: {
                    },
                    yAxis: {
                        type: 'category',
                        data: keys
                    },
                    series: [
                        {
                            name: 'Budget',
                            type: 'bar',
                            data: yearlyBudget
                        },
                        {
                            name: 'Actual',
                            type: 'bar',
                            data: actualBudget
                        }
                    ]
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

