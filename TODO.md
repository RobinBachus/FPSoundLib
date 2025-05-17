# TODO:

- [ ] Move render thread to a separate class (or bridge class)
- [ ] Add a queue to the render thread

## Current imagined flow:

1. Bridge class is created and starts the render thread
2. Player requests a renderer.
3. Any samples passed to the renderer are added to a queue
4. The render thread processes the queue and renders the samples