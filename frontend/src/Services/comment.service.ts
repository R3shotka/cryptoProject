import api from './api.service';

export interface Comment {
    id: number;
    title: string;
    content: string;
    createdOn: string;
    cryptoAssetId?: number;
    createdBy?: string;
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
    getAll: async (): Promise<Comment[]> => {
        const response = await api.get<Comment[]>('/comment');
        return response.data;
    },

    getById: async (id: number): Promise<Comment> => {
        const response = await api.get<Comment>(`/comment/${id}`);
        return response.data;
    },

    create: async (cryptoAssetId: number, data: CreateCommentDto): Promise<Comment> => {
        const response = await api.post<Comment>(`/comment/${cryptoAssetId}`, data);
        return response.data;
    },

    update: async (id: number, data: UpdateCommentDto): Promise<Comment> => {
        const response = await api.put<Comment>(`/comment/${id}`, data);
        return response.data;
    },

    delete: async (id: number): Promise<void> => {
        await api.delete(`/comment/${id}`);
    },
};
