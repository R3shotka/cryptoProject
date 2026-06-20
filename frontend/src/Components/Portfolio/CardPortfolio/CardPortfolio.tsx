import React from 'react';
import { Link } from 'react-router-dom';
import { PortfolioItem } from '../../../Services/portfolio.service';

interface Props {
    portfolioItem: PortfolioItem;
    onPortfolioDelete: (symbol: string) => void;
}

const CardPortfolio: React.FC<Props> = ({ portfolioItem, onPortfolioDelete }) => {
    const isPositive = portfolioItem.change24HPercent.startsWith('+') ||
                       parseFloat(portfolioItem.change24HPercent) > 0;

    return (
        <div className="bg-white rounded-2xl shadow-lg hover:shadow-2xl transition-all duration-300 p-6 border border-gray-100 hover:border-blue-200 group">
            <div className="flex items-center justify-between mb-5">
                <div className="flex items-center gap-3">
                    {portfolioItem.logoUrl && (
                        <div className="w-14 h-14 rounded-full bg-gradient-to-br from-blue-100 to-purple-100 p-2 group-hover:scale-110 transition-transform">
                            <img
                                src={portfolioItem.logoUrl}
                                alt={portfolioItem.name}
                                className="w-full h-full rounded-full"
                            />
                        </div>
                    )}
                    <div>
                        <Link
                            to={`/crypto/${portfolioItem.cryptoAssetId}`}
                            className="text-lg font-bold text-gray-900 hover:text-blue-600 transition-colors"
                        >
                            {portfolioItem.name}
                        </Link>
                        <p className="text-sm text-gray-500 font-medium">{portfolioItem.symbol.toUpperCase()}</p>
                    </div>
                </div>
                <button
                    onClick={() => onPortfolioDelete(portfolioItem.symbol)}
                    className="text-gray-400 hover:text-red-500 hover:bg-red-50 w-8 h-8 rounded-lg transition-all duration-200 text-xl font-bold"
                >
                    ×
                </button>
            </div>

            <div className="space-y-3">
                <div className="flex justify-between items-center py-2 border-b border-gray-100">
                    <span className="text-sm text-gray-500">Quantity</span>
                    <span className="font-semibold text-gray-900">{portfolioItem.quantity.toFixed(8)}</span>
                </div>
                <div className="flex justify-between items-center py-2 border-b border-gray-100">
                    <span className="text-sm text-gray-500">Price</span>
                    <span className="font-semibold text-gray-900">${portfolioItem.currentPrice.toLocaleString()}</span>
                </div>
                <div className="flex justify-between items-center py-2 bg-gradient-to-r from-blue-50 to-purple-50 -mx-6 px-6 rounded-xl mt-3">
                    <span className="text-sm font-medium text-gray-700">Total Value</span>
                    <span className="font-bold text-xl text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-purple-600">
                        ${portfolioItem.totalValue.toLocaleString()}
                    </span>
                </div>
                <div className="flex justify-between items-center py-2">
                    <span className="text-sm text-gray-500">24h Change</span>
                    <span className={`font-bold px-3 py-1 rounded-full text-sm ${isPositive ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'}`}>
                        {portfolioItem.change24HPercent}
                    </span>
                </div>
            </div>
        </div>
    );
};

export default CardPortfolio;
