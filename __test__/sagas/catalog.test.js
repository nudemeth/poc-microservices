import * as effects from 'redux-saga/effects';
import * as actions from '../../actions/catalog.actions';
import * as catalogSaga from '../../sagas/catalog.saga';
import * as catalogTypeSaga from '../../sagas/catalogType.saga';
import catalogApi from '../../api/catalog.api';

describe('Load Item Worker saga', () => {
    it('Should call fetching item by id', () => {
        const params = { payload: { id: 1 } };
        const data = { "id": 1, "name": "Item 1", "catalogTypeId": 1 };
        const generator = catalogSaga.loadItemWorker(params);
        const result = generator.next();
        expect(result.value).toEqual(effects.call(catalogApi.getItemById, params.payload.id));
        expect(result.done).toBeFalsy();
    });

    it('Should put action to loadItemSuccess after successfully fetch item by id', () => {
        const params = { payload: { id: 1 } };
        const data = { "id": 1, "name": "Item 1", "catalogTypeId": 1 };
        const res = { json: () => data };
        const generator = catalogSaga.loadItemWorker(params);
        
        generator.next();
        generator.next(res);

        const result = generator.next(data);
        expect(result.value).toEqual(effects.put(actions.loadItemSuccess(data)));
        expect(result.done).toBeFalsy();
    });

    it('Should put action to loadItemFailure when an error occurs on fetching item by id', () => {
        const params = { payload: { id: 1 } };
        const err = new Error();
        const res = { json: () => { throw err; }};
        const generator = catalogSaga.loadItemWorker(params);
        
        generator.next();
        
        const result = generator.next(res);
        expect(result.value).toEqual(effects.put(actions.loadItemFailure(err)));
        expect(result.done).toBeFalsy();
    });

    it('Should end the generator after putting to loadItemFailure action', () => {
        const params = { payload: { id: 1 } };
        const err = new Error();
        const res = { json: () => { throw err; }};
        const generator = catalogSaga.loadItemWorker(params);
        
        generator.next();
        generator.next(res);

        const result = generator.next();
        expect(result.done).toBeTruthy();
    });

    it('Should end the generator after putting to loadItemSuccess action', () => {
        const params = { payload: { id: 1 } };
        const data = { "id": 1, "name": "Item 1", "catalogTypeId": 1 };
        const res = { json: () => data };
        const generator = catalogSaga.loadItemWorker(params);
        
        generator.next();
        generator.next(res);
        generator.next(data);

        const result = generator.next();
        expect(result.done).toBeTruthy();
    });
});

describe('Load Items Worker saga', () => {
    it('Should call getSelectedCatalogTypeIds', () => {
        const generator = catalogSaga.loadItemsWorker();
        const result = generator.next();
        expect(result.value).toEqual(effects.call(catalogTypeSaga.getSelectedCatalogTypeIds));
        expect(result.done).toBeFalsy();
    });

    it('Should call getItems from item api', () => {
        const catalogTypeIds = [1];
        const generator = catalogSaga.loadItemsWorker();

        generator.next();

        const result = generator.next(catalogTypeIds);
        expect(result.value).toEqual(effects.call(catalogApi.getItems, catalogTypeIds));
        expect(result.done).toBeFalsy();
    });

    it('Should put action to loadItemsSuccess after successfully fetch items by catalogType ids', () => {
        const data = { payload: [{ id: 1 }, { id: 2 }] };
        const res = { json: () => data };
        const generator = catalogSaga.loadItemsWorker();
        
        generator.next();
        generator.next();
        generator.next(res);

        const result = generator.next(data);
        expect(result.value).toEqual(effects.put(actions.loadItemsSuccess(data)));
        expect(result.done).toBeFalsy();
    });

    it('Should put action to loadItemsFailure after failed to fetch items by catalogType ids', () => {
        const err = new Error();
        const res = { json: () => { throw err; } };
        const generator = catalogSaga.loadItemsWorker();
        
        generator.next();
        generator.next();

        const result = generator.next(res);
        expect(result.value).toEqual(effects.put(actions.loadItemsFailure(err)));
        expect(result.done).toBeFalsy();
    });

    it('Should end the generator after putting to loadItemsSuccess action', () => {
        const data = { payload: [{ id: 1 }, { id: 2 }] };
        const res = { json: () => data };
        const generator = catalogSaga.loadItemsWorker();
        
        generator.next();
        generator.next();
        generator.next(res);
        generator.next(data);

        const result = generator.next();
        expect(result.done).toBeTruthy();
    });

    it('Should end the generator after putting to loadItemsFailure action', () => {
        const err = new Error();
        const res = { json: () => { throw err; }};
        const generator = catalogSaga.loadItemsWorker();
        
        generator.next();
        generator.next();
        generator.next(res);

        const result = generator.next();
        expect(result.done).toBeTruthy();
    });
});