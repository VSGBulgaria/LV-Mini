import React, { Component } from "react";
import ReactDOM from "react-dom";
import ReactEcharts from 'echarts-for-react';
import LoanChart from './LoanChart';
import BudgetChart from './BudgetChart';



class Charts extends Component {
    render() {
        return (
            <div>
                <LoanChart />
                <BudgetChart />
            </div>
        );
    }
}

ReactDOM.render(<Charts />, document.getElementById("charts"));