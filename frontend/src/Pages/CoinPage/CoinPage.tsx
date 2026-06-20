import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { cryptoApi } from '../../Services/api.service';
import { commentService, Comment } from '../../Services/comment.service';
import AddComment from '../../Components/Comment/AddComment/AddComment';
import ListComment from '../../Components/Comment/ListComment/ListComment';

const CoinPage = () => {
    const { id } = useParams<{ id: string }>();
    const [cryptoAsset, setCryptoAsset] = useState<any>(null);
    const [comments, setComments] = useState<Comment[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string>('');

    const loadCryptoData = async () => {
        if (!id) return;

        try {
            setLoading(true);
            const data = await cryptoApi.getByIdLive(parseInt(id));
            setCryptoAsset(data);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Failed to load cryptocurrency data');
        } finally {
            setLoading(false);
        }
    };

    const loadComments = async () => {
        if (!id) return;

        try {
            const result = await commentService.getAll({ cryptoAssetId: parseInt(id) });
            setComments(result.data);
        } catch (error) {
            console.error('Failed to load comments', error);
        }
    };

    useEffect(() => {
        loadCryptoData();
        loadComments();
    }, [id]);

    const handleCommentCreate = async (title: string, content: string) => {
        if (!id) return;

        try {
            await commentService.create(parseInt(id), { title, content });
            await loadComments();
        } catch (error: any) {
            alert(error.response?.data?.message || 'Failed to create comment');
        }
    };

    const handleCommentDelete = async (commentId: number) => {
        try {
            await commentService.delete(commentId);
            await loadComments();
        } catch (error) {
            console.error('Failed to delete comment', error);
        }
    };

    if (loading) {
        return <div className="flex justify-center items-center min-h-screen">
            <div className="text-xl text-gray-600">Loading...</div>
        </div>;
    }

    if (error) {
        return <div className="flex justify-center items-center min-h-screen">
            <div className="text-xl text-red-600">{error}</div>
        </div>;
    }

    if (!cryptoAsset) {
        return <div className="flex justify-center items-center min-h-screen">
            <div className="text-xl text-gray-600">Cryptocurrency not found</div>
        </div>;
    }

    const isPositive = cryptoAsset.change24HPercent?.startsWith('+') ||
                       parseFloat(cryptoAsset.change24HPercent) > 0;

    return (
        <div className="container mx-auto px-4 py-8">
            <div className="bg-white rounded-lg shadow-lg p-8 mb-8">
                <div className="flex items-center gap-4 mb-6">
                    {cryptoAsset.logoUrl && (
                        <img src={cryptoAsset.logoUrl} alt={cryptoAsset.name} className="w-16 h-16 rounded-full" />
                    )}
                    <div>
                        <h1 className="text-4xl font-bold text-gray-900">{cryptoAsset.name}</h1>
                        <p className="text-xl text-gray-500">{cryptoAsset.symbol.toUpperCase()}</p>
                    </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                    <div className="bg-blue-50 p-6 rounded-lg">
                        <p className="text-gray-600 text-sm mb-2">Current Price</p>
                        <p className="text-3xl font-bold text-gray-900">${cryptoAsset.price?.toLocaleString() || '0'}</p>
                    </div>
                    <div className="bg-gray-50 p-6 rounded-lg">
                        <p className="text-gray-600 text-sm mb-2">24h Change</p>
                        <p className={`text-3xl font-bold ${isPositive ? 'text-green-600' : 'text-red-600'}`}>
                            {cryptoAsset.change24HPercent || '0%'}
                        </p>
                    </div>
                    <div className="bg-gray-50 p-6 rounded-lg">
                        <p className="text-gray-600 text-sm mb-2">Total Comments</p>
                        <p className="text-3xl font-bold text-gray-900">{comments.length}</p>
                    </div>
                </div>
            </div>

            <div className="bg-white rounded-lg shadow-lg p-8">
                <h2 className="text-2xl font-bold text-gray-900 mb-6">Community Notes</h2>
                <div className="mb-6">
                    <AddComment cryptoAssetId={parseInt(id!)} onCommentCreate={handleCommentCreate} />
                </div>
                <ListComment comments={comments} onDelete={handleCommentDelete} />
            </div>
        </div>
    );
}

export default CoinPage;