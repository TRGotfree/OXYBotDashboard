const path = require('path');
const UglifyJSPlugin = require('uglifyjs-webpack-plugin');
const VueLoaderPlugin = require('vue-loader/lib/plugin');

module.exports = {
    entry: './src/main.js',
    mode: 'development',
    output: {
        path: path.resolve(__dirname, '../wwwroot'),
        publicPath: '/src/',
        filename: 'build.js'
    },
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            }, {
                test: /\.css$/,
                use: [
                    'vue-style-loader',
                    'css-loader'
                ]
            }, {
                test: /.js$/,
                include: [
                    path.resolve(__dirname, "src"),
                    require.resolve("bootstrap-vue"),
                ],
                use: 'babel-loader'
            }
        ]
    },
    plugins: [
        new UglifyJSPlugin(),
        new VueLoaderPlugin()
    ]
}