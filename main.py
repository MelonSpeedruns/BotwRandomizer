import json
import shutil
import os
import glob
import pymsgbox

import oead
import random

from PyQt5 import QtWidgets, QtGui
from PyQt5.QtWidgets import QFileDialog, QApplication

import rstb
import re
import sarc

from randomizer_window import Ui_MainWindow

basePath = ""
updatePath = ""
dlcPath = ""
graphicPacksPath = ""
dlcMainFieldPath = ""
graphicPacksMainFieldPath = ""
corruptedAocMainFieldPath = ""
updateRstbPath = ""
graphicPacksRstbPath = ""
systemVersionPath = ""
rulesPath = ""
titleBgSubPath = ""
titleBgUpdatePath = ""
titleBgGraphicPacksPath = ""
basePackPath = ""
dlcPackPath = ""
graphicPacksPackPath = ""
graphicPacksDlcPackPath = ""
graphicPacksEventPath = ""
updateEventPath = ""
graphicPacksEventFlowPath = ""

object_list = []
events_to_disable_list = []
dungeons = []

files_modified = {}

paraglider_chest = ""
chests_with_spirit_orbs = []


def get_map_files_from_dir(directory, extension):
    map_files = []
    for entry in glob.iglob(f"{directory}/**/*.{extension}", recursive=True):
        if entry.find("-") != -1:
            map_files.append(entry.replace('/', '\\'))

    return map_files


def get_dir_path(path):
    return os.path.dirname(os.path.abspath(path))


def rstb_file():
    with open(graphicPacksRstbPath, "rb") as f:
        rstb_file_data = rstb.ResourceSizeTable(oead.yaz0.decompress(f.read()), True)

        for file in files_modified:
            rstb_file_data.set_size(file, files_modified[file])

    with open(graphicPacksRstbPath, "wb") as f:
        rstb_file_data.write(f, True)

    with open(graphicPacksRstbPath, "rb") as f:
        new_rstb = oead.yaz0.compress(f.read())

    with open(graphicPacksRstbPath, "wb") as f:
        f.write(new_rstb)


def split_on_empty_lines(s):
    blank_line_regex = r"(?:\r?\n){2,}"
    return re.split(blank_line_regex, s.strip())


def remove_from_list(new_value, objects):
    if "Armor_" in new_value:
        objects.replace(new_value + "\n", "")
        return
    if "Horse" in new_value:
        objects.replace(new_value + "\n", "")
        return
    if "Mannequin_" in new_value:
        objects.replace(new_value + "\n", "")
        return


def randomize_element(map_object):
    hash_id = map_object["HashId"]

    for objects in object_list:
        if map_object["UnitConfigName"] in objects:
            object_line = str.splitlines(objects)
            new_value = random.choice(object_line)
            remove_from_list(new_value, objects)
            map_object["UnitConfigName"] = new_value
            break

    if "!Parameters" in map_object:
        if "DropActor" in map_object["!Parameters"]:
            handle_new_value(map_object, "DropActor")
        if "EquipItem1" in map_object["!Parameters"]:
            handle_new_value(map_object, "EquipItem1")
        if "EquipItem2" in map_object["!Parameters"]:
            handle_new_value(map_object, "EquipItem2")
        if "EquipItem3" in map_object["!Parameters"]:
            handle_new_value(map_object, "EquipItem3")
        if "EquipItem4" in map_object["!Parameters"]:
            handle_new_value(map_object, "EquipItem4")
        if "ArrowName" in map_object["!Parameters"]:
            handle_new_value(map_object, "ArrowName")

        global paraglider_chest

        if str(hex(hash_id)) == paraglider_chest:
            set_chest_drop(map_object, "PlayerStole2")
        else:
            if str(hex(hash_id)) in chests_with_spirit_orbs:
                set_chest_drop(map_object, "Obj_DungeonClearSeal")


def handle_new_value(map_object, param_name):
    for objects in object_list:
        if map_object["!Parameters"][param_name] in objects:
            object_line = str.splitlines(objects)
            new_value = random.choice(object_line)
            remove_from_list(new_value, objects)
            map_object["!Parameters"][param_name] = new_value
            return


def set_chest_drop(map_object, drop_value):
    map_object["!Parameters"]["DropActor"] = drop_value


def randomize_files(map_files_path):
    map_files = get_map_files_from_dir(map_files_path, "smubin")
    for map_file in map_files:
        with open(map_file, "rb") as f:
            map_info = oead.byml.from_binary(oead.yaz0.decompress(f.read()))

            for map_index, map_value in enumerate(map_info["Objs"]):
                randomize_element(map_value)

                # Disable Objects
                if "UnitConfigName" in map_info["Objs"][map_index]:
                    if map_info["Objs"][map_index]["UnitConfigName"].startswith("Npc_King_"):
                        map_info["Objs"][map_index] = None

                if map_info["Objs"][map_index] is not None and "HashId" in map_info["Objs"][map_index]:
                    hash_id = map_info["Objs"][map_index]["HashId"]

                    if str(hex(hash_id)) in events_to_disable_list:
                        map_info["Objs"][map_index] = None

            new_byml_binary = oead.byml.to_binary(map_info, True, 2)

            file = map_file.replace('\\', '/') \
                .replace(graphicPacksPath, "") \
                .replace("content\\", "") \
                .replace(".s", ".") \
                .replace("/aoc", "Aoc")

            files_modified[file] = len(new_byml_binary) + 0x200

        new_byml = oead.yaz0.compress(new_byml_binary)

        with open(map_file, "wb") as f:
            f.write(new_byml)


def copy_file(src, dst):
    os.makedirs(get_dir_path(dst), exist_ok=True)
    shutil.copyfile(src, dst)


def copy_map_files(src, extension):
    all_files = get_map_files_from_dir(src, extension)
    for map_file in all_files:
        new_map_location = map_file.replace(dlcMainFieldPath.replace('/', '\\'), graphicPacksMainFieldPath.replace('/', '\\'))
        os.makedirs(get_dir_path(new_map_location), exist_ok=True)
        shutil.copyfile(map_file, new_map_location)


def create_corrupted_aoc_file(file_path):
    os.makedirs(get_dir_path(file_path), exist_ok=True)
    open(file_path, 'a').close()


def create_version_file(file_path):
    os.makedirs(get_dir_path(file_path), exist_ok=True)
    with open(file_path, 'w') as f:
        f.write("Rando 1.0.0")


def create_rules_file():
    bundle_dir = getattr(sys, '_MEIPASS', os.path.abspath(os.path.dirname(__file__)))
    rules = os.path.abspath(os.path.join(bundle_dir, 'rules.txt'))
    copy_file(rules, rulesPath)


def create_object_list():
    bundle_dir = getattr(sys, '_MEIPASS', os.path.abspath(os.path.dirname(__file__)))
    objects = os.path.abspath(os.path.join(bundle_dir, 'objects.txt'))
    with open(objects) as f:
        object_array = split_on_empty_lines(f.read())
        for object_lines in object_array:
            object_list.append(object_lines)

    new_chests = os.path.abspath(os.path.join(bundle_dir, 'chests.txt'))
    with open(new_chests) as f:
        lines = str.splitlines(f.read())
        global chests_with_spirit_orbs
        chests_with_spirit_orbs = random.sample(lines, 300)

    paraglider_chests = os.path.abspath(os.path.join(bundle_dir, 'paraglider_chests.txt'))
    with open(paraglider_chests) as f:
        lines = str.splitlines(f.read())
        global paraglider_chest
        paraglider_chest = str(random.choice(lines))

    events_to_disable = os.path.abspath(os.path.join(bundle_dir, 'events_to_disable.txt'))
    with open(events_to_disable) as f:
        global events_to_disable_list
        events_to_disable_list = str.splitlines(f.read())


def copy_events(event_name):
    old_event_beventpack = updateEventPath + event_name + ".sbeventpack"
    new_event_beventpack = graphicPacksEventPath + event_name + ".sbeventpack"

    copy_file(old_event_beventpack, new_event_beventpack)

    bundle_dir = getattr(sys, '_MEIPASS', os.path.abspath(os.path.dirname(__file__)))
    old_event_bfevfl = os.path.abspath(os.path.join(bundle_dir, 'Events/' + event_name + ".bfevfl"))
    new_event_bfevfl = graphicPacksEventFlowPath + event_name + ".bfevfl"

    copy_file(old_event_bfevfl, new_event_bfevfl)

    with open(new_event_beventpack, "rb") as d:
        archive_bytes = oead.yaz0.decompress(d.read())

    archive = sarc.SARC(archive_bytes)
    writer = sarc.make_writer_from_sarc(archive)

    if archive:
        writer.delete_file("EventFlow/" + event_name + ".bfevfl")

        with open(new_event_beventpack, "wb") as d:
            writer.write(d)

        files_modified["EventFlow/" + event_name + ".bfevfl"] = os.path.getsize(new_event_bfevfl) + 0x200

    with open(new_event_beventpack, "rb") as f:
        new_event_pack = oead.yaz0.compress(f.read())

    with open(new_event_beventpack, "wb") as f:
        f.write(new_event_pack)


def randomize_shrines():
    dungeon_paths = []

    for i in range(0, 120):
        dungeon_name = "Dungeon" + str(i).rjust(3, '0') + ".pack"
        copy_file(basePackPath + dungeon_name, graphicPacksPackPath + dungeon_name)
        dungeon_paths.append(graphicPacksPackPath + dungeon_name)

    for i in range(120, 137):
        dungeon_name = "Dungeon" + str(i).rjust(3, '0') + ".pack"
        copy_file(dlcPackPath + dungeon_name, graphicPacksDlcPackPath + dungeon_name)
        dungeon_paths.append(graphicPacksDlcPackPath + dungeon_name)

    for dungeon in dungeon_paths:
        with open(dungeon, "rb") as d:
            archive_bytes = d.read()

        archive = sarc.SARC(archive_bytes)
        writer = sarc.make_writer_from_sarc(archive)

        if archive:
            for file_name in archive.list_files():
                if file_name.endswith(".smubin"):
                    data = archive.get_file_data(file_name)

                    dungeon_data = oead.byml.from_binary(oead.yaz0.decompress(data))

                    for map_index, map_value in enumerate(dungeon_data["Objs"]):
                        randomize_element(map_value)

                        # Disable Objects
                        if "HashId" in dungeon_data["Objs"][map_index]:
                            hash_id = dungeon_data["Objs"][map_index]["HashId"]

                            if str(hex(hash_id)) in events_to_disable_list:
                                dungeon_data["Objs"][map_index] = None

                    new_byml_binary = oead.byml.to_binary(dungeon_data, True, 2)

                    files_modified[file_name] = len(new_byml_binary) + 0x200

                    new_byml = oead.yaz0.compress(new_byml_binary)

                    writer.delete_file(file_name)
                    writer.add_file(file_name, new_byml)

            with open(dungeon, "wb") as d:
                writer.write(d)


def randomize(log_box):
    log_to_console(log_box, "Randomization Started!")

    # Graphic Pack Creation
    log_to_console(log_box, "Creating Graphic Pack...")
    create_object_list()
    if os.path.exists(graphicPacksPath):
        shutil.rmtree(graphicPacksPath)
    copy_file(updateRstbPath, graphicPacksRstbPath)
    copy_map_files(dlcMainFieldPath, "smubin")
    create_rules_file()
    create_version_file(systemVersionPath)
    create_corrupted_aoc_file(corruptedAocMainFieldPath)
    copy_events("Demo003_0")
    copy_events("Demo700_0")
    copy_events("Demo701_0")
    copy_events("Demo033_0")
    copy_events("HyruleCastle")

    # Randomization Process
    log_to_console(log_box, "Randomizing Overworld...")
    randomize_files(graphicPacksMainFieldPath)

    log_to_console(log_box, "Randomizing Shrines...")
    randomize_shrines()

    log_to_console(log_box, "Updating File Size Table...")
    rstb_file()

    log_to_console(log_box, "Randomization Completed!")


class MainWindow(QtWidgets.QMainWindow, Ui_MainWindow):
    def __init__(self, parent=None):
        QtWidgets.QMainWindow.__init__(self, parent=parent)

        self.setupUi(self)

        self.setFixedSize(800, 380)

        icon_dir = getattr(sys, '_MEIPASS', "icon.png")
        self.setWindowIcon(QtGui.QIcon(icon_dir))

        self.settings_dict = {"baseFolder": '',
                              "updateFolder": '',
                              "dlcFolder": '',
                              "graphicPacksFolder": '',
                              }

        if not os.path.exists("settings.ini"):
            with open("settings.ini", "w+") as json_file:
                json.dump(self.settings_dict, json_file)
        else:
            with open("settings.ini", "r") as json_file:
                self.settings_dict = json.load(json_file)

        self.baseFolder.setText(self.settings_dict["baseFolder"])
        self.updateFolder.setText(self.settings_dict["updateFolder"])
        self.dlcFolder.setText(self.settings_dict["dlcFolder"])
        self.graphicPacksFolder.setText(self.settings_dict["graphicPacksFolder"])

        self.pushButton.setEnabled(False)

        if self.baseFolder.text() is not "" and self.updateFolder.text() is not "" and self.dlcFolder.text() is not "" and self.graphicPacksFolder.text() is not "":
            self.pushButton.setEnabled(True)

        self.browseButton1.clicked.connect(lambda: self.browse_folder('Select the base BOTW "content" folder', self.baseFolder, "baseFolder"))
        self.browseButton2.clicked.connect(lambda: self.browse_folder('Select the update BOTW "content" folder', self.updateFolder, "updateFolder"))
        self.browseButton3.clicked.connect(lambda: self.browse_folder('Select the DLC BOTW "content" folder', self.dlcFolder, "dlcFolder"))
        self.browseButton4.clicked.connect(lambda: self.browse_folder('Select the Cemu "graphicPacks" folder', self.graphicPacksFolder, "graphicPacksFolder"))

        self.pushButton.clicked.connect(lambda: self.randomization())

        self.show()

    def browse_folder(self, text, text_box, settings_value):
        dialog = QFileDialog()
        base_dir = dialog.getExistingDirectory(self, text)

        if settings_value == "graphicPacksFolder":
            if os.path.basename(base_dir) != "graphicPacks":
                pymsgbox.alert("The folder you have selected wasn't named \"graphicPacks\".", "Wrong Directory!")
                base_dir = ""
        else:
            if os.path.basename(base_dir) != "content":
                pymsgbox.alert("The folder you have selected wasn't named \"content\".", "Wrong Directory!")
                base_dir = ""

        text_box.setText(base_dir)

        self.settings_dict[settings_value] = text_box.text()
        with open("settings.ini", "w+") as json_file:
            json.dump(self.settings_dict, json_file)

        if self.baseFolder.text() is not "" and self.updateFolder.text() is not "" and self.dlcFolder.text() is not "" and self.graphicPacksFolder.text() is not "":
            self.pushButton.setEnabled(True)
        else:
            self.pushButton.setEnabled(False)

    def randomization(self):
        global basePath
        global updatePath
        global dlcPath
        global graphicPacksPath

        basePath = self.baseFolder.text()
        updatePath = self.updateFolder.text()
        dlcPath = self.dlcFolder.text()
        graphicPacksPath = self.graphicPacksFolder.text() + "/BOTW Randomizer/"

        global dlcMainFieldPath
        dlcMainFieldPath = dlcPath + "/0010/Map/MainField"

        global graphicPacksMainFieldPath
        graphicPacksMainFieldPath = graphicPacksPath + "/aoc/0010/Map/MainField"

        global corruptedAocMainFieldPath
        corruptedAocMainFieldPath = graphicPacksPath + "/aoc/0010/Pack/AocMainField.pack"

        global updateRstbPath
        updateRstbPath = updatePath + "/System/Resource/ResourceSizeTable.product.srsizetable"

        global graphicPacksRstbPath
        graphicPacksRstbPath = graphicPacksPath + "/content/System/Resource/ResourceSizeTable.product.srsizetable"

        global systemVersionPath
        systemVersionPath = graphicPacksPath + "/content/System/Version.txt"

        global rulesPath
        rulesPath = graphicPacksPath + "/rules.txt"

        global titleBgSubPath
        titleBgSubPath = "/Pack/TitleBG.pack"

        global titleBgUpdatePath
        titleBgUpdatePath = updatePath + titleBgSubPath

        global titleBgGraphicPacksPath
        titleBgGraphicPacksPath = graphicPacksPath + "/content/" + titleBgSubPath

        global basePackPath
        basePackPath = basePath + "/Pack/"

        global dlcPackPath
        dlcPackPath = dlcPath + "/0010/Pack/"

        global graphicPacksPackPath
        graphicPacksPackPath = graphicPacksPath + "/content/Pack/"

        global graphicPacksDlcPackPath
        graphicPacksDlcPackPath = graphicPacksPath + "/aoc/0010/Pack/"

        global updateEventPath
        updateEventPath = updatePath + "/Event/"

        global graphicPacksEventPath
        graphicPacksEventPath = graphicPacksPath + "/content/Event/"

        global graphicPacksEventFlowPath
        graphicPacksEventFlowPath = graphicPacksPath + "/content/EventFlow/"

        self.logBox.clear()
        QApplication.processEvents()
        randomize(self.logBox)


def log_to_console(log_box, text):
    log_box.append(text + "\n")
    QApplication.processEvents()


if __name__ == "__main__":
    import sys

    app = QApplication([])
    window = MainWindow()
    sys.exit(app.exec_())
