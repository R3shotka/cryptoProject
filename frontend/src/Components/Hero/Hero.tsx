import React from 'react'
import { Link } from 'react-router-dom'
import BitcoinChart from '../BitcoinChart/BitcoinChart'

interface Props { }

const Hero = (props: Props) => {
    return (
        <section id="hero" className="bg-gradient-to-br from-gray-50 to-blue-50 min-h-screen flex items-center">
            <div className="container mx-auto px-6 lg:px-12">
                <div className="flex flex-col-reverse lg:flex-row items-center gap-12">
                    <div className="flex-1 space-y-8 text-center lg:text-left">
                        <h1 className="text-5xl lg:text-6xl xl:text-7xl font-extrabold text-gray-900 leading-tight">
                            Track Your <span className="text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-purple-600">Crypto Portfolio</span>
                        </h1>
                        <p className="text-xl lg:text-2xl text-gray-600 max-w-2xl mx-auto lg:mx-0">
                            Real-time cryptocurrency tracking with live prices from CoinGecko.
                            Manage your investments, add notes, and watch your portfolio grow.
                        </p>
                        <div className="flex flex-col sm:flex-row gap-4 justify-center lg:justify-start">
                            <Link
                                to="/register"
                                className="px-8 py-4 text-lg font-bold text-white bg-gradient-to-r from-blue-600 to-purple-600 rounded-xl hover:shadow-2xl hover:scale-105 transition-all duration-300"
                            >
                                Get Started Free
                            </Link>
                            <Link
                                to="/search"
                                className="px-8 py-4 text-lg font-bold text-gray-700 bg-white border-2 border-gray-300 rounded-xl hover:border-blue-500 hover:shadow-lg transition-all duration-300"
                            >
                                Explore Crypto
                            </Link>
                        </div>
                        <div className="flex gap-8 justify-center lg:justify-start pt-4">
                            <div>
                                <p className="text-3xl font-bold text-gray-900">1000+</p>
                                <p className="text-sm text-gray-500">Cryptocurrencies</p>
                            </div>
                            <div>
                                <p className="text-3xl font-bold text-gray-900">Real-time</p>
                                <p className="text-sm text-gray-500">Live Prices</p>
                            </div>
                            <div>
                                <p className="text-3xl font-bold text-gray-900">Free</p>
                                <p className="text-sm text-gray-500">Always</p>
                            </div>
                        </div>
                    </div>
                    <div className="flex-1 flex justify-center w-full">
                        <div className="w-full max-w-2xl">
                            <BitcoinChart />
                        </div>
                    </div>
                </div>
            </div>
        </section>
    )
}

export default Hero