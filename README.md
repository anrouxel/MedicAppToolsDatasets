<h1 align="center">
  <img src="MedicApp.png" alt="MedicAppToolsDatasets" height="200">
  <br />
</h1>

<p align="center"><b>This project is licensed under the EUPL 1.2. For more details, see the <a href="LICENSE.md">LICENSE</a> file.</b></p>
<p align="center"><i>"A Toolkit that downloads and cleans french medication database"</i></p>

<p align="center">
<a href="https://snapcraft.io/medicapptoolsdatasets">
  <img alt="medicapptoolsdatasets" src="https://snapcraft.io/medicapptoolsdatasets/badge.svg" />
</a>
<a href="https://snapcraft.io/medicapptoolsdatasets">
  <img alt="medicapptoolsdatasets" src="https://snapcraft.io/medicapptoolsdatasets/trending.svg?name=0" />
</a>
</p>

## Install

```shell
snap install medicapptoolsdatasets
```

<a href="https://snapcraft.io/medicapptoolsdatasets">
  <img alt="Get it from the Snap Store" src="https://snapcraft.io/static/images/badges/en/snap-store-black.svg" />
</a>

([Don't have snapd installed?](https://snapcraft.io/docs/core/install))

<p align="center">Published for <img src="https://raw.githubusercontent.com/anythingcodes/slack-emoji-for-techies/gh-pages/emoji/tux.png" align="top" width="24" /> with :gift_heart: by anrouxel</p>


## Usage

```plaintext
medicapptoolsdatasets [options]
```

## Options

```plaintext
    -m, --model <model>: Model to use - ai or dataset
    -d, --dataset <dataset>: Dataset to use - CIS_bdpm, CIS_COMPO_bdpm, CIS_CIP_bdpm, CIS_GENER_bdpm, CIS_HAS_SMR_bdpm, CIS_HAS_ASMR_bdpm, CIS_InfoImportantes, CIS_CPD_bdpm, HAS_LiensPageCT_bdpm, all
    --merge: Merge datasets
    -o, --outputDir <outputDir>: Output directory
    -u, --outputUrl <outputUrl>: Output URL
    -s, --sentenceCount <sentenceCount>: Number of sentences to generate
    -i, --input <input>: Input file
    --version: Show version information
    -?, -h, --help: Show help and usage information
```

## Example

Here are some examples of how to use `medicapptoolsdatasets`:

### Merging all datasets and outputting to a specific directory

```plaintext
medicapptoolsdatasets -m dataset -d all --merge -o /path/to/outputDir
```


### Merging all datasets and outputting to a specific URL

```plaintext
medicapptoolsdatasets -m dataset -d all --merge -u http://example.com/output
```


### Outputting all datasets to a specific directory

```plaintext
medicapptoolsdatasets -m dataset -d all -o /path/to/outputDir
```


### Outputting all datasets to a specific URL

```plaintext
medicapptoolsdatasets -m dataset -d all -u http://example.com/output
```


### Outputting the CIS_bdpm dataset to a specific directory

```plaintext
medicapptoolsdatasets -m dataset -d CIS_bdpm -o /path/to/outputDir
```


### Outputting the CIS_bdpm dataset to a specific URL

```plaintext
medicapptoolsdatasets -m dataset -d CIS_bdpm -u http://example.com/output
```


### Outputting the CIS_bdpm and CIS_COMPO_bdpm datasets to a specific directory

```plaintext
medicapptoolsdatasets -m dataset -d CIS_bdpm -d CIS_COMPO_bdpm -o /path/to/outputDir
```


### Outputting the CIS_bdpm and CIS_COMPO_bdpm datasets to a specific URL

```plaintext
medicapptoolsdatasets -m dataset -d CIS_bdpm -d CIS_COMPO_bdpm -u http://example.com/output
```


### Using the AI model with all datasets, generating 100 sentences from an input file and outputting to a specific directory

```plaintext
medicapptoolsdatasets -m ai -d all -s 100 -i /path/to/inputFile -o /path/to/outputDir
```


Please replace `/path/to/outputDir`, `/path/to/inputFile`, and `http://example.com/output` with your actual file paths and URLs.


## Authors
* Antonin Rouxel ([anrouxel](https://github.com/anrouxel))
* Quentin Tegny ([Qt144](https://github.com/Qt144))
* Paul BOUTET ([Kartinlage](https://github.com/Kartinlage))
* Arthur Osselin ([tuturita](https://github.com/tuturita))
* Lana Heyrendt ([Renarde-dev](https://github.com/Renarde-dev))

## Related Git repository
* Android application: [MedicApp](https://github.com/anrouxel/MedicApp#----)
* C# data cleaner: [MedicAppToolsDatasets](https://github.com/anrouxel/MedicAppToolsDatasets#----)
* Artificial Intelligence Generator (Named Entity Recognition): [MedicAppAI](https://github.com/anrouxel/MedicAppAI#----)
* Named Entity Recognition AI model for prescriptions (usable as a sub-module): [MedicAppAssets](https://gitlab.univ-nantes.fr/E213726L/MedicAppAssets.git)
* API : [MedicAppAPI](https://github.com/Renarde-dev/MedicApp-API#----)
