import React, { ChangeEvent, SyntheticEvent, useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom';
import api, { cryptoApi } from '../../Services/api.service';
import { portfolioService } from '../../Services/portfolio.service';
import AddPortfolio from '../../Components/Portfolio/AddPortfolio/AddPortfolio';

interface CryptoSearchResult {
    id?: number;
    symbol: string;
    name: string;
    externalId: string;
    logoUrl?: string;
}

interface TopCrypto {
    externalId: string;
    symbol: string;
    name: string;
    logoUrl?: string;
    price: number;
    change24HPercent: number;
}

interface Props { }

const SearchPage = (props: Props) => {
    const navigate = useNavigate();
    const [search, setSearch] = useState<string>("");
    const [searchResult, setSearchResult] = useState<CryptoSearchResult[]>([]);
    const [topCryptos, setTopCryptos] = useState<TopCrypto[]>([]);
    const [serverError, setServerError] = useState<string>("");
    const [loading, setLoading] = useState<boolean>(false);
    const [loadingTop, setLoadingTop] = useState<boolean>(true);
    const [successMessage, setSuccessMessage] = useState<string>("");

    useEffect(() => {
        loadTopCryptos();
    }, []);

    const loadTopCryptos = async () => {
        try {
            setLoadingTop(true);
            const response = await api.get<TopCrypto[]>('/crypto/top?limit=10');
            setTopCryptos(response.data);
        } catch (error) {
            console.error('Failed to load top cryptocurrencies', error);
        } finally {
            setLoadingTop(false);
        }
    };

    const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
        setSearch(e.target.value);
    }

    const onSearchSubmit = async (e: SyntheticEvent) => {
        e.preventDefault();
        setServerError("");
        setLoading(true);

        try {
            const result = await cryptoApi.search(search);
            setSearchResult(result);
        } catch (error: any) {
            setServerError(error.response?.data?.message || 'Failed to search cryptocurrencies');
        } finally {
            setLoading(false);
        }
    }

    const handlePortfolioCreate = async (symbol: string, quantity: number) => {
        try {
            await portfolioService.addToPortfolio({ symbol, quantity });
            setSuccessMessage(`Added ${quantity} ${symbol.toUpperCase()} to portfolio!`);
            setTimeout(() => setSuccessMessage(""), 3000);
        } catch (error: any) {
            setServerError(error.response?.data?.message || 'Failed to add to portfolio');
        }
    };

    const handleCryptoClick = async (crypto: CryptoSearchResult | TopCrypto) => {
        // Якщо є ID в БД - відразу переходимо
        if ('id' in crypto && crypto.id) {
            navigate(`/crypto/${crypto.id}`);
            return;
        }

        // Якщо немає в БД - додаємо її спочатку
        try {
            const newCrypto = await cryptoApi.create({
                symbol: crypto.symbol,
                name: crypto.name,
                externalId: crypto.externalId,
                price: 0,
                change24HPercent: 0
            });
            navigate(`/crypto/${newCrypto.id}`);
        } catch (error: any) {
            setServerError(error.response?.data?.message || 'Failed to add cryptocurrency');
        }
    };

    return (
        <div className="min-h-screen bg-gradient-to-br from-gray-50 to-blue-50 py-12">
            <div className="container mx-auto px-6">
                <div className="max-w-4xl mx-auto">
                    <h1 className="text-4xl lg:text-5xl font-extrabold text-gray-900 mb-4 text-center">
                        Search <span className="text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-purple-600">Cryptocurrencies</span>
                    </h1>
                    <p className="text-gray-600 text-center mb-8">Find and add cryptocurrencies to your portfolio</p>

                    <form onSubmit={onSearchSubmit} className="mb-8">
                        <div className="flex gap-3">
                            <input
                                type="text"
                                value={search}
                                onChange={handleSearchChange}
                                placeholder="Search Bitcoin, Ethereum, etc..."
                                className="flex-1 px-6 py-4 rounded-xl border-2 border-gray-200 focus:border-blue-500 focus:outline-none text-lg transition-colors"
                            />
                            <button
                                type="submit"
                                disabled={loading}
                                className="px-8 py-4 bg-gradient-to-r from-blue-600 to-purple-600 text-white font-bold rounded-xl hover:shadow-xl transition-all disabled:opacity-50"
                            >
                                {loading ? 'Searching...' : 'Search'}
                            </button>
                        </div>
                    </form>

                    {serverError && (
                        <div className="bg-red-50 border-2 border-red-200 text-red-800 px-6 py-4 rounded-xl mb-6">
                            {serverError}
                        </div>
                    )}

                    {successMessage && (
                        <div className="bg-green-50 border-2 border-green-200 text-green-800 px-6 py-4 rounded-xl mb-6">
                            ✓ {successMessage}
                        </div>
                    )}

                    {/* Результати пошуку */}
                    {searchResult.length > 0 && (
                        <div className="space-y-4 mb-8">
                            <h2 className="text-2xl font-bold text-gray-900">Search Results</h2>
                            {searchResult.map((crypto) => (
                                <div
                                    key={crypto.externalId}
                                    className="bg-white p-6 rounded-2xl shadow-md hover:shadow-xl transition-all border border-gray-100 hover:border-blue-200"
                                >
                                    <div className="flex items-center justify-between">
                                        <div
                                            className="flex items-center gap-4 cursor-pointer flex-1"
                                            onClick={() => handleCryptoClick(crypto)}
                                        >
                                            {crypto.logoUrl && (
                                                <div className="w-14 h-14 rounded-full bg-gradient-to-br from-blue-100 to-purple-100 p-2">
                                                    <img src={crypto.logoUrl} alt={crypto.name} className="w-full h-full rounded-full" />
                                                </div>
                                            )}
                                            <div>
                                                <h3 className="text-xl font-bold text-gray-900 hover:text-blue-600 transition-colors">{crypto.name}</h3>
                                                <p className="text-sm text-gray-500 font-medium">{crypto.symbol.toUpperCase()}</p>
                                            </div>
                                        </div>
                                        <AddPortfolio symbol={crypto.symbol} onPortfolioCreate={handlePortfolioCreate} />
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}

                    {/* Топ криптовалюти */}
                    {!search && (
                        <div className="space-y-4">
                            <h2 className="text-2xl font-bold text-gray-900">Top Cryptocurrencies</h2>
                            {loadingTop ? (
                                <div className="text-center py-10 text-gray-500">Loading top cryptocurrencies...</div>
                            ) : (
                                topCryptos.map((crypto) => {
                                    const isPositive = crypto.change24HPercent >= 0;
                                    return (
                                        <div
                                            key={crypto.externalId}
                                            className="bg-white p-6 rounded-2xl shadow-md hover:shadow-xl transition-all border border-gray-100 hover:border-blue-200"
                                        >
                                            <div className="flex items-center justify-between">
                                                <div
                                                    className="flex items-center gap-4 cursor-pointer flex-1"
                                                    onClick={() => handleCryptoClick(crypto)}
                                                >
                                                    {crypto.logoUrl && (
                                                        <div className="w-14 h-14 rounded-full bg-gradient-to-br from-blue-100 to-purple-100 p-2">
                                                            <img src={crypto.logoUrl} alt={crypto.name} className="w-full h-full rounded-full" />
                                                        </div>
                                                    )}
                                                    <div className="flex-1">
                                                        <h3 className="text-xl font-bold text-gray-900 hover:text-blue-600 transition-colors">{crypto.name}</h3>
                                                        <p className="text-sm text-gray-500 font-medium">{crypto.symbol.toUpperCase()}</p>
                                                    </div>
                                                    <div className="text-right mr-4">
                                                        <p className="text-xl font-bold text-gray-900">${crypto.price.toLocaleString()}</p>
                                                        <p className={`text-sm font-semibold ${isPositive ? 'text-green-600' : 'text-red-600'}`}>
                                                            {isPositive ? '+' : ''}{crypto.change24HPercent.toFixed(2)}%
                                                        </p>
                                                    </div>
                                                </div>
                                                <AddPortfolio symbol={crypto.symbol} onPortfolioCreate={handlePortfolioCreate} />
                                            </div>
                                        </div>
                                    );
                                })
                            )}
                        </div>
                    )}

                    {!loading && searchResult.length === 0 && search && (
                        <div className="text-center py-20">
                            <div className="text-6xl mb-4">🔍</div>
                            <p className="text-xl text-gray-500">No results found for "{search}"</p>
                        </div>
                    )}
                </div>
            </div>
        </div>
    )
}

export default SearchPage