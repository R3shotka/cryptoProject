import axios from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5144/api';

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token && config.headers) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export interface RegisterData {
    username: string;
    email: string;
    password: string;
}

export interface LoginData {
    userName: string;
    password: string;
}

export interface UserResponse {
    userName: string;
    email: string;
    token: string;
}

export const authApi = {
    register: async (data: RegisterData): Promise<UserResponse> => {
        const response = await api.post<UserResponse>('/Account/register', data);
        return response.data;
    },

    login: async (data: LoginData): Promise<UserResponse> => {
        const response = await api.post<UserResponse>('/Account/login', data);
        return response.data;
    },
};

export interface CryptoAsset {
    id: number;
    symbol: string;
    name: string;
    externalId: string;
    logoUrl?: string;
    price: number;
    change24HPercent: string;
}

export interface CreateCryptoAsset {
    symbol: string;
    name: string;
    externalId: string;
    price: number;
    change24HPercent: number;
}

export const cryptoApi = {
    getAll: async (): Promise<CryptoAsset[]> => {
        const response = await api.get<CryptoAsset[]>('/CryptoAsset');
        return response.data;
    },

    getById: async (id: number): Promise<CryptoAsset> => {
        const response = await api.get<CryptoAsset>(`/CryptoAsset/${id}`);
        return response.data;
    },

    getByIdLive: async (id: number): Promise<CryptoAsset> => {
        const response = await api.get<CryptoAsset>(`/CryptoAsset/${id}/live`);
        return response.data;
    },

    create: async (data: CreateCryptoAsset): Promise<CryptoAsset> => {
        const response = await api.post<CryptoAsset>('/CryptoAsset', data);
        return response.data;
    },

    search: async (query: string): Promise<any[]> => {
        const response = await api.get<any[]>(`/Crypto/search?query=${query}`);
        return response.data;
    },
};

export default api;
