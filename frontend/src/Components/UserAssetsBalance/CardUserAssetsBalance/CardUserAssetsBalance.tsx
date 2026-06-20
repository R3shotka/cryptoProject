import React, { SyntheticEvent } from 'react'
import DeleteUserAssetsBalance from '../DeleteUserAssetaBalance/DeleteUserAssetsBalance';
import { Link } from 'react-router-dom';

interface Props {
    UserAssetsBalanceValue: string;
    onUserAssetsBalanceDelete: (e: SyntheticEvent) => void;
}

const CardUserAssetsBalance = ({ UserAssetsBalanceValue, onUserAssetsBalanceDelete }: Props) => {
    return (
        <div className="flex flex-col w-full p-8 space-y-4 text-center rounded-lg shadow-lg md:w-1/3">
            <Link to={`/crypto/${UserAssetsBalanceValue}`} className="pt-6 text-xl font-bold" ></Link>
            {UserAssetsBalanceValue}
            <DeleteUserAssetsBalance onUserAssetsBalanceDelete={onUserAssetsBalanceDelete} UserAssetsBalanceValue={UserAssetsBalanceValue} />
        </div>
    )
}

export default CardUserAssetsBalance