import { Injectable, Renderer2, RendererFactory2 } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private renderer: Renderer2;
  private _currentTheme: 'light' | 'dark' = 'light';

  get currentTheme() {
    return this._currentTheme;
  }

  set currentTheme(theme: 'light' | 'dark') {
    this._currentTheme = theme;
  }

  constructor(rendererFactory: RendererFactory2) {
    this.renderer = rendererFactory.createRenderer(null, null);
    const savedTheme = localStorage.getItem('theme') as 'light' | 'dark';
    this.setTheme(savedTheme || 'light');
  }

  setTheme(theme: 'light' | 'dark') {
    this._currentTheme = theme;
    this.renderer.setAttribute(document.documentElement, 'data-theme', theme);
    localStorage.setItem('theme', theme);
  }

  toggleTheme() {
    this.setTheme(this._currentTheme === 'light' ? 'dark' : 'light');
  }
}
