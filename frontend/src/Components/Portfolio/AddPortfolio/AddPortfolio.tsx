import React, { useState } from 'react';

interface Props {
    symbol: string;
    onPortfolioCreate: (symbol: string, quantity: number) => void;
}

const AddPortfolio: React.FC<Props> = ({ symbol, onPortfolioCreate }) => {
    const [quantity, setQuantity] = useState<string>('1');
    const [showInput, setShowInput] = useState(false);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        const qty = parseFloat(quantity);
        if (qty > 0) {
            onPortfolioCreate(symbol, qty);
            setQuantity('1');
            setShowInput(false);
        }
    };

    if (!showInput) {
        return (
            <button
                onClick={() => setShowInput(true)}
                className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition"
            >
                Add to Portfolio
            </button>
        );
    }

    return (
        <form onSubmit={handleSubmit} className="flex gap-2 items-center">
            <input
                type="number"
                step="0.00000001"
                min="0.00000001"
                value={quantity}
                onChange={(e) => setQuantity(e.target.value)}
                placeholder="Quantity"
                className="w-32 px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
                autoFocus
            />
            <button
                type="submit"
                className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600 transition"
            >
                Add
            </button>
            <button
                type="button"
                onClick={() => setShowInput(false)}
                className="px-4 py-2 bg-gray-300 text-gray-700 rounded hover:bg-gray-400 transition"
            >
                Cancel
            </button>
        </form>
    );
};

export default AddPortfolio;
