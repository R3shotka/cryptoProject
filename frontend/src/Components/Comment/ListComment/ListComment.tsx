import React from 'react';
import CardComment from '../CardComment/CardComment';
import { Comment } from '../../../Services/comment.service';

interface Props {
    comments: Comment[];
    onDelete: (id: number) => void;
}

const ListComment: React.FC<Props> = ({ comments, onDelete }) => {
    if (comments.length === 0) {
        return (
            <div className="text-center py-8 text-gray-500">
                <p>No notes yet. Add your first note!</p>
            </div>
        );
    }

    return (
        <div className="space-y-3">
            {comments.map((comment) => (
                <CardComment key={comment.id} comment={comment} onDelete={onDelete} />
            ))}
        </div>
    );
};

export default ListComment;
