import axios from 'axios';
import { CryptoSearch } from './crypto';


interface SearchCryptoResponse {
    coins: CryptoSearch[];
}
export const searchCrypto = async (query: string) => {
    try {
        const response = await axios.get<SearchCryptoResponse>(`https://api.coingecko.com/api/v3/search?query=${query}`);
        return response.data.coins;
    }
    catch (error) {
        // Явно приводимо помилку до типу AxiosError
        const err = error as any;

        // Перевіряємо властивість isAxiosError на самому об'єкті, а не через axios.isAxiosError()
        if (err.isAxiosError) {
            console.log('Axios error:', err.message);
            return err.message; // Повертаємо повідомлення про помилку як рядок
        } else {
            console.log('Unexpected error:', error);
            return 'An unexpected error occurred'; // Повертаємо загальне повідомлення про помилку
        }

    }
}

export const getCryptoProfile = async (id: string) => {
    try {
        const response = await axios.get(`https://api.coingecko.com/api/v3/coins/${id}`);
        return response.data;
    }
    catch (error) {
        const err = error as any;

        if (err.isAxiosError) {
            console.log('Axios error:', err.message);
            return err.message; // Повертаємо повідомлення про помилку як рядок
        } else {
            console.log('Unexpected error:', error);
            return 'An unexpected error occurred'; // Повертаємо загальне повідомлення про помилку
        }
    }
}