import React from 'react'

interface CoinData {
    symbol?: string;
    market_data?: {
        current_price?: { usd?: number };
        price_change_percentage_24h?: number;
        market_cap?: { usd?: number };
    };
}

type Props = {
    coinData?: CoinData;
}

const CoinDashboard = ({ coinData }: Props) => {
    return (
        <div className="relative md:ml-64 bg-slate-100 w-full min-h-screen">
            <div className="relative pt-20 pb-32 bg-blue-500">
                <div className="px-4 md:px-6 mx-auto w-full">
                    <div>
                        <div className="flex flex-wrap">

                            <div className="w-full lg:w-6/12 xl:w-3/12 px-4">
                                <div className="relative flex flex-col min-w-0 break-words bg-white rounded-lg mb-6 xl:mb-0 shadow-lg">
                                    <div className="flex-auto p-4">
                                        <div className="flex flex-wrap">
                                            <div className="relative w-full pr-4 max-w-full flex-grow flex-1">
                                                <h5 className="text-slate-400 uppercase font-bold text-xs">
                                                    Symbol
                                                </h5>
                                                <span className="font-bold text-xl">{coinData?.symbol?.toUpperCase()}</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div className="w-full lg:w-6/12 xl:w-3/12 px-4">
                                <div className="relative flex flex-col min-w-0 break-words bg-white rounded-lg mb-6 xl:mb-0 shadow-lg">
                                    <div className="flex-auto p-4">
                                        <div className="flex flex-wrap">
                                            <div className="relative w-full pr-4 max-w-full flex-grow flex-1">
                                                <h5 className="text-slate-400 uppercase font-bold text-xs">
                                                    Current Price
                                                </h5>
                                                <span className="font-bold text-xl">
                                                    ${coinData?.market_data?.current_price?.usd?.toLocaleString() || "0.00"}
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div className="w-full lg:w-6/12 xl:w-3/12 px-4">
                                <div className="relative flex flex-col min-w-0 break-words bg-white rounded-lg mb-6 xl:mb-0 shadow-lg">
                                    <div className="flex-auto p-4">
                                        <div className="flex flex-wrap">
                                            <div className="relative w-full pr-4 max-w-full flex-grow flex-1">
                                                <h5 className="text-slate-400 uppercase font-bold text-xs">
                                                    Change (24h)
                                                </h5>
                                                <span className={`font-bold text-xl ${(coinData?.market_data?.price_change_percentage_24h || 0) >= 0 ? 'text-green-500' : 'text-red-500'}`}>
                                                    {coinData?.market_data?.price_change_percentage_24h?.toFixed(2) || "0.00"}%
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div className="w-full lg:w-6/12 xl:w-3/12 px-4">
                                <div className="relative flex flex-col min-w-0 break-words bg-white rounded-lg mb-6 xl:mb-0 shadow-lg">
                                    <div className="flex-auto p-4">
                                        <div className="flex flex-wrap">
                                            <div className="relative w-full pr-4 max-w-full flex-grow flex-1">
                                                <h5 className="text-slate-400 uppercase font-bold text-xs">
                                                    Market Cap
                                                </h5>
                                                <span className="font-bold text-xl">
                                                    ${coinData?.market_data?.market_cap?.usd?.toLocaleString() || "0"}
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default CoinDashboard