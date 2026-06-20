import React from 'react';
import { Comment } from '../../../Services/comment.service';

interface Props {
    comment: Comment;
    onDelete: (id: number) => void;
}

const CardComment: React.FC<Props> = ({ comment, onDelete }) => {
    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        return date.toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    return (
        <div className="bg-gray-50 p-4 rounded-lg border border-gray-200">
            <div className="flex justify-between items-start mb-2">
                <div>
                    <h4 className="font-bold text-gray-900">{comment.title}</h4>
                    <p className="text-xs text-blue-600 font-medium">by {comment.createdBy || 'Anonymous'}</p>
                </div>
                <button
                    onClick={() => onDelete(comment.id)}
                    className="text-red-500 hover:text-red-700 text-xl font-bold"
                >
                    ×
                </button>
            </div>
            <p className="text-gray-700 mb-2">{comment.content}</p>
            <p className="text-xs text-gray-500">{formatDate(comment.createdOn)}</p>
        </div>
    );
};

export default CardComment;
