var path = require('path');
var prod = (process.env.NODE_ENV || '').trim() === 'production';

module.exports = {

    context: path.resolve(__dirname, 'wwwroot/js/main/'),
    entry: {
        myProfile: ["./my-profile/my-profile-index"],
        teams: ["./teams/teams-index"],
        users: ["./users/users-index"]
        
    },
    output: {
        path: path.resolve(__dirname, 'wwwroot/js/build'),
        filename: '[name].bundle.js',
        chunkFilename: "[chunkhash].bundle.js",
        publicPath: prod ? 'wwwroot/js/build/' : 'http://localhost:9000/build/'
    },
    devServer: {
        contentBase: path.join(__dirname, "dist"),
        compress: true,
        port: 9000
    },
    devtool: !prod ? 'eval-source-map' : 'source-map',
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                loaders: ['babel-loader']
            },
            {
                test: /\.jsx$/,
                exclude: /node_modules/,
                loaders: ['babel-loader']
            }
        ]
    },
    resolve: {
        //modulesDirectories: ['.', 'node-modules'],
        extensions: ['*', '.js', '.jsx']
    }

};