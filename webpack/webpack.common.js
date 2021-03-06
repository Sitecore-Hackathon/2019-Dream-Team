const Path = require('path');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
    entry: {
        app: Path.resolve(__dirname, '../src/Project/Website/client/index.js')
    },
    output: {
        path: Path.join(__dirname, '../build'),
        filename: 'js/[name].js',
    },
    optimization: {
        splitChunks: {
            cacheGroups: {
                js: {
                    test: /\.js$/,
                    name: "vendor",
                    chunks: "all",
                },
                css: {
                    test: /\.(css|sass|scss)$/,
                    name: "vendor",
                    chunks: "all",
                }
            }
        }
    },
    plugins: [
        new CleanWebpackPlugin(['build'], {root: Path.resolve(__dirname, '..')}),
        new CopyWebpackPlugin([
            {from: Path.resolve(__dirname, '../src/Project/Website/client/media'), to: 'media'}
        ]),
    ],
    resolve: {
        alias: {
            '~': Path.resolve(__dirname, '../src')
        }
    },
    module: {
        rules: [
            {
                test: /\.mjs$/,
                include: /node_modules/,
                type: 'javascript/auto'
            },
            {
                test: /\.(ico|jpg|jpeg|png|gif|eot|otf|webp|svg|ttf|woff|woff2)(\?.*)?$/,
                use: {
                    loader: 'file-loader',
                    options: {
                        name: '[path][name].[ext]'
                    }
                }
            },
        ]
    }
};
