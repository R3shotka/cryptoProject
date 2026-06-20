export interface CryptoSearch {
    id: string;       // наприклад 'bitcoin' (це потрібно для запитів на CoinGecko)
    symbol: string;   // 'btc'
    name: string;     // 'Bitcoin'
    thumb: string;    // url маленької картинки
}

// Це для детальної сторінки монети (аналог його CompanyProfile)
export interface CryptoProfile {
    id: string;
    symbol: string;
    name: string;
    description: string;
    logoUrl: string;
    currentPrice: number;
    marketCap: number;
    totalVolume: number;
    priceChangePercentage24h: number;
}