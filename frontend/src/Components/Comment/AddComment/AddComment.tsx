import React, { useState } from 'react';

interface Props {
    cryptoAssetId: number;
    onCommentCreate: (title: string, content: string) => void;
}

const AddComment: React.FC<Props> = ({ cryptoAssetId, onCommentCreate }) => {
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');
    const [showForm, setShowForm] = useState(false);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (title.trim() && content.trim()) {
            onCommentCreate(title, content);
            setTitle('');
            setContent('');
            setShowForm(false);
        }
    };

    if (!showForm) {
        return (
            <button
                onClick={() => setShowForm(true)}
                className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition"
            >
                + Add Note
            </button>
        );
    }

    return (
        <form onSubmit={handleSubmit} className="bg-white p-4 rounded-lg shadow space-y-3">
            <h3 className="font-bold text-lg">Add Note</h3>
            <input
                type="text"
                placeholder="Title"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
                maxLength={200}
                required
            />
            <textarea
                placeholder="Your thoughts..."
                value={content}
                onChange={(e) => setContent(e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none"
                rows={4}
                maxLength={5000}
                required
            />
            <div className="flex gap-2">
                <button
                    type="submit"
                    className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600 transition"
                >
                    Save
                </button>
                <button
                    type="button"
                    onClick={() => setShowForm(false)}
                    className="px-4 py-2 bg-gray-300 text-gray-700 rounded hover:bg-gray-400 transition"
                >
                    Cancel
                </button>
            </div>
        </form>
    );
};

export default AddComment;
