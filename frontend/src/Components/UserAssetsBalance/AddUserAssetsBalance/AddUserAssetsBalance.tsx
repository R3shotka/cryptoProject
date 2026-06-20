import React, { SyntheticEvent } from 'react'

interface Props {
    onUserAssetsBalanceCreate: (e: SyntheticEvent) => void;
    symbol: string;
}

const AddUserAssetsBalance = ({ onUserAssetsBalanceCreate, symbol }: Props) => {
    return (
        <div className="flex flex-col items-center justify-end flex-1 space-x-4 space-y-2 md:flex-row md:space-y-0">
            <form onSubmit={onUserAssetsBalanceCreate}>
                {/* Зберіг name="symbol", це важливо для твоєї логіки! */}
                <input readOnly={true} hidden={true} name="symbol" value={symbol} />

                <button
                    type="submit"
                    className="p-2 px-8 text-white bg-blue-800 rounded-lg hover:opacity-70 focus:outline-none transition-opacity"
                >
                    Add
                </button>
            </form>
        </div>
    )
}

export default AddUserAssetsBalance