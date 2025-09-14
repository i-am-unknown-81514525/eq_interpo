# EquationInterpolation

A TUI program which interpolate a curve with a polynomial formula using [Newton Polynomial method](https://en.wikipedia.org/wiki/Newton_polynomial) from the given set of point

### How to use
- For testing
  - Input a series of `x` and `y` value from a curve that you know
  - Click submit and then next
  - Click on the formula to copy it (It is expected that it might not be in your clipboard, in this case, copy the value from `.clipboard`)
  - Compare the interpolated graph to the actual graph in Desmos (You can just pasted it into Desmos since the copy output is output as Latex)
- For actual usage
  - Input a series of `x` and `y` value from a curve that you know
  - Click submit and see if you see any value that seem abnormal
  - Go back and toggle those entries off if you believe it cause issue
  - Similar to how you do testing, but you compare against the curve to what roughly should happen (For example, if you expect a somewhat linear relationship but those point just keep going up and down because inaccuracy in the data collection)
  - Repeat this with enabling and disabling some entries to see if you can find what you want


### Feature List
- [x] The basic (obviously)
- [x] Display step for the table
- [x] Output the equation for human readable form
- [x] Copy to clipboard on clicking the formula generated from interpolation in latex form **Note that it midn not always work**
- [x] Write to `.clipboard` as well for the generated formula
- [x] Autobuild with [Github Action](https://github.com/i-am-unknown-81514525/eq_interpo/actions/workflows/full_build.yaml) for Windows/Linux/macOS in x86_64/ARM64
- [ ] Method to disable writing to `.clipboard` (for situation where it can copy correctly)
- [x] Allow deactivatng point from being used for interpolationn
- [x] Calculate Loss based on the point not used to interpolate the graph
- [ ] Better UI
- [ ] Pasting list of input at once
- [ ] Horizontal Paging for the output table when it is too wide
- [x] Precise math calculation by not using float etc.
- [ ] Find replacement for checkbox font (It doesn't currently work on some machine and show a `?`, notably Windows, use the color to interpret it instead)

### Video demo

https://github.com/user-attachments/assets/00b86861-3e6e-4a80-ae2c-2828e8d15f0a

Followup update made after the demo:
- Calculating MAE (Mean Absolute Error) (Example came from interpolating f(x)=sqrt(x)
<img width="2702" height="1180" alt="image" src="https://github.com/user-attachments/assets/3ed5360a-8e3f-4e74-a5bc-4de99bd2e298" />
<img width="2704" height="1184" alt="image" src="https://github.com/user-attachments/assets/fdb3b145-890a-491e-aa6b-fa4299a60e51" />
<img width="2802" height="1768" alt="image" src="https://github.com/user-attachments/assets/815b8d5e-c1d6-47c4-821b-568e9cf503a7" />
(From the output, the MAE calculated is 0.33748880222804523 if I didn't type it incorrectly in the calculator)

