import React from 'react';
import ReactEcharts from 'echarts-for-react';  // or var ReactEcharts = require('echarts-for-react');

<ReactEcharts
option={this.getOption()}
notMerge={true}
lazyUpdate={true}
theme={"theme_name"}
onChartReady={this.onChartReadyCallback}
onEvents={EventsDict}
opts={} />