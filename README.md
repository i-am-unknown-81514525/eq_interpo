# EquationInterpolation

A TUI program which interpolate a curve with a polynomial formula using [Newton Polynomial method](https://en.wikipedia.org/wiki/Newton_polynomial) from the given set of point

### Feature List
- [x] The basic (obviously)
- [x] Display step for the table
- [x] Output the equation for human readable form
- [x] Copy to clipboard on clicking the formula generated from interpolation in latex form **Note that it midn not always work**
- [x] Write to `.clipboard` as well for the generated formula
- [x] Autobuild with [Github Action](https://github.com/i-am-unknown-81514525/eq_interpo/actions/workflows/full_build.yaml) for Windows/Linux/macOS in x86_64/ARM64
- [ ] Method to disable writing to `.clipboard` (for situation where it can copy correctly)
- [x] Allow deactivatng point from being used for interpolationn
- [ ] Calculate Loss based on the point not used to interpolate the graph
- [ ] Better UI
- [ ] Pasting list of input at once
- [ ] Horizontal Paging for the output table when it is too wide
- [x] Precise math calculation by not using float etc.
