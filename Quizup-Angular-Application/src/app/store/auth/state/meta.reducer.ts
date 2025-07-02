import { ActionReducer, INIT, UPDATE } from '@ngrx/store';

export function localStorageMetaReducer<S>(reducer: ActionReducer<S>): ActionReducer<S> {
  return (state, action) => {
    // On app init, load state from localStorage
    if (action.type === INIT || action.type === UPDATE) {
      const storageValue = localStorage.getItem('appState');
      if (storageValue) {
        return JSON.parse(storageValue);
      }
    }
    const nextState = reducer(state, action);
    localStorage.setItem('appState', JSON.stringify(nextState));
    return nextState;
  };
}