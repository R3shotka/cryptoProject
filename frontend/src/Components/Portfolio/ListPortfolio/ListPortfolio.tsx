import React from 'react';
import CardPortfolio from '../CardPortfolio/CardPortfolio';
import { PortfolioItem } from '../../../Services/portfolio.service';

interface Props {
    portfolioItems: PortfolioItem[];
    onPortfolioDelete: (symbol: string) => void;
}

const ListPortfolio: React.FC<Props> = ({ portfolioItems, onPortfolioDelete }) => {
    if (portfolioItems.length === 0) {
        return (
            <div className="text-center py-20 bg-gradient-to-br from-gray-50 to-blue-50 rounded-3xl">
                <div className="text-6xl mb-4">📊</div>
                <p className="text-2xl font-bold text-gray-700 mb-2">Your portfolio is empty</p>
                <p className="text-gray-500">Search for cryptocurrencies and start building your portfolio</p>
            </div>
        );
    }

    const totalValue = portfolioItems.reduce((sum, item) => sum + item.totalValue, 0);

    return (
        <div>
            <div className="bg-gradient-to-br from-blue-600 via-purple-600 to-indigo-700 rounded-3xl shadow-2xl p-8 mb-10 text-white relative overflow-hidden">
                <div className="absolute top-0 right-0 w-64 h-64 bg-white opacity-5 rounded-full -mr-32 -mt-32"></div>
                <div className="absolute bottom-0 left-0 w-48 h-48 bg-white opacity-5 rounded-full -ml-24 -mb-24"></div>

                <div className="relative z-10">
                    <h2 className="text-xl font-semibold mb-2 opacity-90">Total Portfolio Value</h2>
                    <p className="text-5xl md:text-6xl font-extrabold mb-4">
                        ${totalValue.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                    </p>
                    <div className="flex items-center gap-6">
                        <div>
                            <p className="text-sm opacity-80">Assets</p>
                            <p className="text-2xl font-bold">{portfolioItems.length}</p>
                        </div>
                        <div className="h-12 w-px bg-white opacity-30"></div>
                        <div>
                            <p className="text-sm opacity-80">Status</p>
                            <p className="text-2xl font-bold">✓ Active</p>
                        </div>
                    </div>
                </div>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-6">
                {portfolioItems.map((item) => (
                    <CardPortfolio
                        key={item.cryptoAssetId}
                        portfolioItem={item}
                        onPortfolioDelete={onPortfolioDelete}
                    />
                ))}
            </div>
        </div>
    );
};

export default ListPortfolio;
