import api from './api.service';

export interface PortfolioItem {
    cryptoAssetId: number;
    symbol: string;
    name: string;
    quantity: number;
    currentPrice: number;
    totalValue: number;
    change24HPercent: string;
    logoUrl?: string;
}

export interface AddPortfolioRequest {
    symbol: string;
    quantity: number;
}

export const portfolioService = {
    getPortfolio: async (): Promise<PortfolioItem[]> => {
        const response = await api.get<PortfolioItem[]>('/Portfolio');
        return response.data;
    },

    addToPortfolio: async (data: AddPortfolioRequest): Promise<PortfolioItem> => {
        const response = await api.post<PortfolioItem>('/Portfolio', data);
        return response.data;
    },

    removeFromPortfolio: async (symbol: string): Promise<void> => {
        await api.delete(`/Portfolio?symbol=${symbol}`);
    },
};
