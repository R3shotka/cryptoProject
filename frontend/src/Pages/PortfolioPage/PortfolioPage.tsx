import React, { useEffect, useState } from 'react';
import { portfolioService, PortfolioItem } from '../../Services/portfolio.service';
import ListPortfolio from '../../Components/Portfolio/ListPortfolio/ListPortfolio';

const PortfolioPage: React.FC = () => {
    const [portfolio, setPortfolio] = useState<PortfolioItem[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string>('');

    const loadPortfolio = async () => {
        try {
            setLoading(true);
            const data = await portfolioService.getPortfolio();
            setPortfolio(data);
            setError('');
        } catch (err: any) {
            setError(err.response?.data?.message || 'Failed to load portfolio');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadPortfolio();
    }, []);

    const handlePortfolioDelete = async (symbol: string) => {
        try {
            await portfolioService.removeFromPortfolio(symbol);
            await loadPortfolio();
        } catch (err: any) {
            setError(err.response?.data?.message || 'Failed to remove from portfolio');
        }
    };

    if (loading) {
        return (
            <div className="flex justify-center items-center min-h-screen">
                <div className="text-xl text-gray-600">Loading portfolio...</div>
            </div>
        );
    }

    return (
        <div className="container mx-auto px-4 py-8">
            <h1 className="text-4xl font-bold mb-8 text-gray-900">My Portfolio</h1>
            {error && (
                <div className="bg-red-50 border border-red-200 text-red-800 px-4 py-3 rounded mb-4">
                    {error}
                </div>
            )}
            <ListPortfolio portfolioItems={portfolio} onPortfolioDelete={handlePortfolioDelete} />
        </div>
    );
};

export default PortfolioPage;
