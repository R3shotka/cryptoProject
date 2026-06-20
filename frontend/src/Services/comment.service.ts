import api from './api.service';

export interface Comment {
    id: number;
    title: string;
    content: string;
    createdOn: string;
    cryptoAssetId?: number;
    createdBy?: string;
}

export interface PaginatedComments {
    data: Comment[];
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
}

export interface CreateCommentDto {
    title: string;
    content: string;
}

export interface UpdateCommentDto {
    title: string;
    content: string;
}

export const commentService = {
    getAll: async (params?: { cryptoAssetId?: number; page?: number; pageSize?: number }): Promise<PaginatedComments> => {
        const response = await api.get<PaginatedComments>('/Comment', { params });
        return response.data;
    },

    getById: async (id: number): Promise<Comment> => {
        const response = await api.get<Comment>(`/Comment/${id}`);
        return response.data;
    },

    create: async (cryptoAssetId: number, data: CreateCommentDto): Promise<Comment> => {
        const response = await api.post<Comment>(`/Comment/${cryptoAssetId}`, data);
        return response.data;
    },

    update: async (id: number, data: UpdateCommentDto): Promise<Comment> => {
        const response = await api.put<Comment>(`/Comment/${id}`, data);
        return response.data;
    },

    delete: async (id: number): Promise<void> => {
        await api.delete(`/Comment/${id}`);
    },
};
