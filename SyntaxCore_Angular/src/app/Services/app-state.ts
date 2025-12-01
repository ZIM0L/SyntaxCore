import { createAction, createReducer, on } from '@ngrx/store'

export interface AppState {
  consoleState: 'intro' | 'tutorial' |'open' | 'closed'; 
  errorMessage: string;
    status: 'pending' | 'loading' | 'error' | 'success' | 'idle';
    userPreferences: {
      theme: 'light' | 'dark';
  }
}
const initialState: AppState = {
  consoleState: 'intro',
  errorMessage: '',
  status: 'idle',
  userPreferences: {
    theme: 'dark',
  }
};

export const initializeAppState = createAction('[App] Initialize State');
export const resetAppState = createAction('[App] Reset State');

export const appStateReducer = createReducer(initialState,
  on(initializeAppState, state => ({ ...initialState }))
);
